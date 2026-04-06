import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Auth } from '../../services/auth/auth';

@Component({
  selector: 'app-home',
  imports: [CommonModule, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  city = '';

  constructor(private router: Router, public auth: Auth) {}

  onSearch(): void {
    const value = this.city.trim();
    this.router.navigate(['/hotels'], {
      queryParams: value ? { city: value } : {},
    });
  }
}
