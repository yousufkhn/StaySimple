import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Home } from './pages/home/home';
import { adminGuard, publicOnlyGuard, userGuard } from './guards/auth-guard';
import { Hotels } from './pages/hotels/hotels';
import { HotelDetails } from './pages/hotels/hotel-details/hotel-details';
import { BookingConfirmation } from './pages/bookings/booking-confirmation/booking-confirmation';
import { MyBookings } from './pages/bookings/my-bookings/my-bookings';
import { ManageHotels } from './pages/admin/manage-hotels/manage-hotels';
import { ManageRooms } from './pages/admin/manage-rooms/manage-rooms';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login, canActivate: [publicOnlyGuard] },
  { path: 'register', component: Register, canActivate: [publicOnlyGuard] },
  { path: 'hotels', component: Hotels, canActivate: [userGuard] },
  { path: 'hotels/:id', component: HotelDetails, canActivate: [userGuard] },
  { path: 'booking-confirmation', component: BookingConfirmation, canActivate: [userGuard] },
  { path: 'my-bookings', component: MyBookings, canActivate: [userGuard] },
  { path: 'admin/manage-hotels', component: ManageHotels, canActivate: [adminGuard] },
  { path: 'admin/manage-rooms', component: ManageRooms, canActivate: [adminGuard] },
];
