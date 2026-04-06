import { Component,signal } from '@angular/core';
import { Hotel, Room } from '../../../models/models';
import { HotelService, RoomService } from '../../../services/hotel/hotel-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-rooms',
  imports: [CommonModule,FormsModule],
  templateUrl: './manage-rooms.html',
  styleUrl: './manage-rooms.css',
})
export class ManageRooms {
  hotels = signal<Hotel[]>([]);
  rooms = signal<Room[]>([]);
  loading = signal(false);
  saving = signal(false);
  filterHotel = signal(0);
  form = { hotelId: 0, roomType: '', pricePerNight: 2000, capacity: 2, isAvailable: true };

  constructor(private hotelSvc: HotelService, private roomSvc: RoomService) {}

  ngOnInit(): void {
    this.hotelSvc.getAll().subscribe(h => { this.hotels.set(h); this.loadRooms(); });
  }

  loadRooms(): void {
    this.loading.set(true);
    if (this.filterHotel() > 0) {
      this.roomSvc.getByHotel(this.filterHotel()).subscribe({ next: r => { this.rooms.set(r); this.loading.set(false); }, error: () => this.loading.set(false) });
    } else {
      // Load all rooms from all hotels
      const all: Room[] = [];
      let done = 0;
      if (!this.hotels().length) { this.loading.set(false); return; }
      this.hotels().forEach(h => {
        this.roomSvc.getByHotel(h.id).subscribe({ next: r => { all.push(...r); if (++done === this.hotels().length) { this.rooms.set(all); this.loading.set(false); } }, error: () => { if (++done === this.hotels().length) { this.rooms.set(all); this.loading.set(false); } } });
      });
    }
  }

  addRoom(): void {
    if (!this.form.hotelId || !this.form.roomType) return;
    this.saving.set(true);
    this.roomSvc.create({ hotelId: this.form.hotelId, roomType: this.form.roomType, pricePerNight: this.form.pricePerNight, capacity: this.form.capacity, isAvailable: this.form.isAvailable }).subscribe({ next: () => { this.saving.set(false); this.loadRooms(); }, error: () => this.saving.set(false) });
  }

  deleteRoom(id: number): void {
    if (!confirm('Delete this room?')) return;
    this.roomSvc.delete(id).subscribe({ next: () => this.loadRooms() });
  }
}
