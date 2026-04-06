import { Component,signal } from '@angular/core';
import { Hotel } from '../../../models/models';
import { HotelService } from '../../../services/hotel/hotel-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-hotels',
  imports: [CommonModule, FormsModule],
  templateUrl: './manage-hotels.html',
  styleUrl: './manage-hotels.css',
})
export class ManageHotels {
  hotels = signal<Hotel[]>([]); 
  loading = signal(true); 
  saving = signal(false); 
  editing = signal<Hotel|null>(null);
  form = { name: '', city: '', address: '', description: '', rating: 4.0, imageUrl: '' };

  constructor(private svc: HotelService) {}
  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading.set(true);
    this.svc.getAll().subscribe({ next: d => { this.hotels.set(d); this.loading.set(false); }, error: () => this.loading.set(false) });
  }

  edit(h: Hotel): void { this.editing.set(h); this.form = { name: h.name, city: h.city, address: h.address, description: h.description, rating: h.rating, imageUrl: h.imageUrl }; }
  cancelEdit(): void { this.editing.set(null); this.form = { name: '', city: '', address: '', description: '', rating: 4.0, imageUrl: '' }; }

  save(): void {
    this.saving.set(true);
    const req = this.editing() ? this.svc.update(this.editing()!.id, this.form) : this.svc.create(this.form);
    req.subscribe({ next: () => { this.saving.set(false); this.cancelEdit(); this.load(); }, error: () => this.saving.set(false) });
  }

  delete(id: number): void {
    if (!confirm('Delete this hotel?')) return;
    this.svc.delete(id).subscribe({ next: () => this.load() });
  }
}
