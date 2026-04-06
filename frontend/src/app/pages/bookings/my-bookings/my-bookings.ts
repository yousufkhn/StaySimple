import { Component,signal } from '@angular/core';
import { Booking } from '../../../models/models';
import { BookingService } from '../../../services/hotel/hotel-service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-my-bookings',
  imports: [CommonModule,RouterModule],
  templateUrl: './my-bookings.html',
  styleUrl: './my-bookings.css',
})
export class MyBookings {
  bookings = signal<Booking[]>([]);
   loading = signal(true);
   cancelId = signal<number | null>(null);

  get confirmedCount() { return this.bookings().filter(b => b.status === 'Confirmed').length; }
  get cancelledCount() { return this.bookings().filter(b => b.status === 'Cancelled').length; }
  get totalSpent() { return this.bookings().filter(b => b.status !== 'Cancelled').reduce((s, b) => s + b.totalPrice, 0); }

  constructor(private svc: BookingService) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading.set(true);
    this.svc.getMyBookings().subscribe({ next: d => { this.bookings.set(d); this.loading.set(false); }, error: () => this.loading.set(false) });
    console.log(this.bookings());
  }

  cancel(id: number): void {
    if (!confirm('Cancel this booking?')) return;
    this.cancelId.set(id);
    this.svc.cancel(id).subscribe({ next: () => { this.cancelId.set(null); this.load(); }, error: () => this.cancelId.set(null) });
  }

  daysUntil(d: string): number {
    return Math.max(0, Math.ceil((new Date(d).getTime() - Date.now()) / 86400000));
  }
}
