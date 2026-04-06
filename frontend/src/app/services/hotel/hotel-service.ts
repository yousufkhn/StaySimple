import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environments';
import { Observable } from 'rxjs';
import { Hotel,Room,Booking,CreateBooking } from '../../models/models';
import { Auth } from '../auth/auth';

const baseUrl = environment.apiBaseUrl;
@Injectable({
  providedIn: 'root',
})
export class HotelService {
  constructor(private http: HttpClient) {}
  getAll(): Observable<Hotel[]> { 
    return this.http.get<Hotel[]>(`${baseUrl}/hotels`); 
  }
  getById(id: number): Observable<Hotel> { return this.http.get<Hotel>(`${baseUrl}/hotels/${id}`); }
  search(city: string): Observable<Hotel[]> { return this.http.get<Hotel[]>(`${baseUrl}/hotels/search?city=${encodeURIComponent(city)}`); }
  create(h: Partial<Hotel>): Observable<Hotel> { return this.http.post<Hotel>(`${baseUrl}/hotels`, h); }
  update(id: number, h: Partial<Hotel>): Observable<Hotel> { return this.http.put<Hotel>(`${baseUrl}/hotels/${id}`, h); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${baseUrl}/hotels/${id}`); }
}

@Injectable({ providedIn: 'root' })
export class RoomService {
  constructor(private http: HttpClient) {}
  getByHotel(hotelId: number): Observable<Room[]> { return this.http.get<Room[]>(`${baseUrl}/rooms/hotel/${hotelId}`); }
  create(r: Partial<Room>): Observable<Room> { return this.http.post<Room>(`${baseUrl}/rooms`, r); }
  update(id: number, r: Partial<Room>): Observable<Room> { return this.http.put<Room>(`${baseUrl}/rooms/${id}`, r); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${baseUrl}/rooms/${id}`); }
}

@Injectable({ providedIn: 'root' })
export class BookingService {
  private bookingBaseUrl = 'http://localhost:5274/api/bookings';

  constructor(private http: HttpClient, private auth: Auth) {}

  private authOptions(): { headers?: HttpHeaders } {
    const token = this.auth.getToken();
    if (!token) {
      return {};
    }

    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
  }

  getAll(): Observable<Booking[]> {
    return this.http.get<Booking[]>(this.bookingBaseUrl, this.authOptions());
  }

  getMyBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.bookingBaseUrl}/my`, this.authOptions());
  }

  create(b: CreateBooking): Observable<Booking> {
    return this.http.post<Booking>(this.bookingBaseUrl, b, this.authOptions());
  }

  cancel(id: number): Observable<any> {
    return this.http.put(`${this.bookingBaseUrl}/cancel/${id}`, {}, this.authOptions());
  }
}