import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';
import { HotelService } from '../../services/hotel/hotel-service';
import { Hotel } from '../../models/models';


@Component({
  selector: 'app-hotel-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './hotels.html',
  styleUrl: './hotels.css',
})
export class Hotels implements OnInit, OnDestroy {
  hotels = signal<Hotel[]>([]);
  searchCity = signal('');
  loading = signal(false);
  error = signal('');
  sortBy = signal('');

  readonly cities = ['Mumbai', 'Goa', 'Jaipur', 'Manali', 'Delhi', 'Udaipur'];

  private hotelsSub?: Subscription;

  constructor(
    private hotelService: HotelService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParamMap.subscribe((params) => {
      const city = (params.get('city') || '').trim();
      this.searchCity.set(city);
      this.fetchHotels(city || null);
    });
  }

  ngOnDestroy(): void {
    this.hotelsSub?.unsubscribe();
  }

  search(): void {
    const city = this.searchCity().trim();
    this.fetchHotels(city || null);
  }

  clearSearch(): void {
    this.searchCity.set('');
    this.fetchHotels(null);
  }

  sort(): void {
    this.applySort();
  }

  stars(rating: number): number[] {
    return Array(Math.max(0 , Math.round(rating || 0))).fill(0);
  }

  private fetchHotels(city: string | null): void {
    this.hotelsSub?.unsubscribe();
    this.loading.set(true);
    this.error.set('');

    const request$ = city
      ? this.hotelService.search(city)
      : this.hotelService.getAll();

    this.hotelsSub = request$.subscribe({
      next: (data) => {
        this.hotels.set(data ?? []);
        this.applySort();
        this.loading.set(false);
      },
      error: (err) => {
        this.hotels.set([]);
        this.error.set(err?.error?.message || err?.message || 'Failed to load hotels.');
        this.loading.set(false);
      },
    });
  }

  private applySort(): void {
    if (this.sortBy() === 'rating') {
      this.hotels.set([...this.hotels()].sort((a, b) => b.rating - a.rating));
      return;
    }

    if (this.sortBy() === 'name') {
      this.hotels.set([...this.hotels()].sort((a, b) => a.name.localeCompare(b.name)));
    }
  }
}
