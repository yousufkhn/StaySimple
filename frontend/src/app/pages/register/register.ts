import { Component } from '@angular/core';
import { Auth } from '../../services/auth/auth';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  name: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  loading: boolean = false;
  show: boolean = false;
  touched: boolean = false;
  error: string = '';
  success: string = '';

  constructor(private auth: Auth, private router: Router) {}

  onSubmit(): void {
    this.touched = true;
    if (!this.name || !this.email || !this.password || !this.confirmPassword) return;
    if (this.password !== this.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }
    const name = this.name.trim();
    const email = this.email.trim().toLowerCase();
    const password = this.password;

    if (!name || !email || !password) {
      this.error = 'Please fill all required fields.';
      return;
    }

    this.loading = true;
    this.error = '';
    this.success = '';
    this.auth.register({ name, email, password }).subscribe({
      next: () => {
        this.loading = false;
        this.success = 'Registration successful! Redirecting to login...';
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Registration failed.';
      }
    });
  }
}
