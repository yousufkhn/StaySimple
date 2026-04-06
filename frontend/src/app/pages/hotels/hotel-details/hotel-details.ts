import { CommonModule } from '@angular/common';
import { Component, OnInit,signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Hotel, Room } from '../../../models/models';
import { ActivatedRoute, Router } from '@angular/router';

import { HotelService, RoomService, BookingService } from '../../../services/hotel/hotel-service';
import { Auth } from '../../../services/auth/auth';

@Component({
  selector: 'app-hotel-details',
  standalone: true,
  imports: [CommonModule,FormsModule,RouterModule],
  templateUrl: './hotel-details.html',
  styleUrl: './hotel-details.css',
})
export class HotelDetails implements OnInit {
  hotel = signal<Hotel | null>(null);
  rooms = signal<Room[]>([]);
  loading = signal(true);
  roomsLoading = signal(false);
  selected = signal<Room | null>(null);
  checkIn = signal('');
  checkOut = signal('');
  bookingLoading = signal(false);
  bookingError = signal('');
  today = new Date().toISOString().split('T')[0];

  get nights(): number {
    const checkIn = this.checkIn();
    const checkOut = this.checkOut();
    if (!checkIn || !checkOut) return 0;
    return Math.max(0, Math.ceil((new Date(checkOut).getTime() - new Date(checkIn).getTime()) / 86400000));
  }
  get total(): number { return this.nights * (this.selected()?.pricePerNight ?? 0); }

  constructor(private route: ActivatedRoute, private router: Router,
    private hotels: HotelService, private rooms_: RoomService, private bookings: BookingService, public auth: Auth) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.hotels.getById(id).subscribe({
      next: (h) => {
        this.hotel.set(h);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
    this.roomsLoading.set(true);
    this.rooms_.getByHotel(id).subscribe({
      next: (r) => {
        this.rooms.set(r ?? []);
        this.roomsLoading.set(false);
      },
      error: () => this.roomsLoading.set(false),
    });
  }

  select(r: Room): void {
    this.selected.set(r);
    this.bookingError.set('');
  }
  stars(n: number): number[] { return Array(Math.round(n)).fill(0); }

  book(): void {
    if (this.auth.isAdmin()) {
      this.bookingError.set('Admin accounts cannot create bookings.');
      return;
    }

    const selected = this.selected();
    if (!selected || this.nights <= 0) {
      this.bookingError.set('Please select valid dates.');
      return;
    }

    this.bookingLoading.set(true);
    this.bookingError.set('');
    this.bookings.create({
      roomId: selected.id,
      roomType: selected.roomType,
      hotelName: this.hotel()?.name ?? '',
      checkInDate: this.checkIn(),
      checkOutDate: this.checkOut(),
      pricePerNight: selected.pricePerNight
    }).subscribe({
      next: (b) => {
        this.bookingLoading.set(false);
        this.router.navigate(['/booking-confirmation'], {
          state: { hotelName: this.hotel()?.name, roomType: selected.roomType,
            checkIn: this.checkIn(), checkOut: this.checkOut(),
            total: this.total, nights: this.nights, bookingRef: b.bookingRef }
        });
      },
      error: (err) => {
        this.bookingLoading.set(false);
        this.bookingError.set(err.error?.message || 'Booking failed.');
      }
    });
  }
}
