import { Component, signal } from '@angular/core';
import { BookingService } from '../../../services/hotel/hotel-service';
import { Booking } from '../../../models/models';
import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-view-bookings',
  imports: [CommonModule],
  templateUrl: './view-bookings.html',
  styleUrl: './view-bookings.css',
})
export class ViewBookings {
  bookings = signal<Booking[]>([]);
  loading = signal(true);
  cancelId = signal<number | null>(null);
  activeFilter = signal('All');
  search = signal('');
  filters = signal(['All', 'Confirmed', 'Cancelled']);

  get stats() {
    return [
      { label: 'Total', value: this.bookings().length },
      { label: 'Confirmed', value: this.bookings().filter(b => b.status === 'Confirmed').length },
      { label: 'Cancelled', value: this.bookings().filter(b => b.status === 'Cancelled').length },
      { label: 'Revenue (₹)', value: this.bookings().filter(b => b.status !== 'Cancelled').reduce((s, b) => s + b.totalPrice, 0).toLocaleString('en-IN') }
    ];
  }

  get filtered() {
    return this.bookings().filter(b =>
      (this.activeFilter() === 'All' || b.status === this.activeFilter()) &&
      (!this.search() || b.hotelName.toLowerCase().includes(this.search().toLowerCase()) || b.userName.toLowerCase().includes(this.search().toLowerCase()))
    );
  }

  constructor(private svc: BookingService) {}
  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading.set(true);
    this.svc.getAll().subscribe({ next: d => { this.bookings.set(d); this.loading.set(false); }, error: () => this.loading.set(false) });
  }

  cancel(id: number): void {
    if (!confirm('Cancel this booking? A BookingCancelledEvent will be fired to RabbitMQ.')) return;
    this.cancelId.set(id);
    this.svc.cancel(id).subscribe({ next: () => { this.cancelId.set(null); this.load(); }, error: () => this.cancelId.set(null) });
  }
}
