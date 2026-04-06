import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-booking-confirmation',
  imports: [DatePipe],
  templateUrl: './booking-confirmation.html',
  styleUrl: './booking-confirmation.css',
})
export class BookingConfirmation {
  state: any;
  constructor(private router: Router) {
    this.state = this.router.getCurrentNavigation()?.extras.state ?? {};
  }
}
