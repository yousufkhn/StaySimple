import { Component } from '@angular/core';
import { Auth } from '../../services/auth/auth';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  email: string = '';
  password: string = '';
  loading: boolean = false;
  show : boolean = false;
  touched : boolean = false;
  error : string = '';

  constructor(private auth : Auth,private router : Router) {}

  onSubmit(): void {
    this.touched = true;
    if (!this.email || !this.password) return;
    const email = this.email.trim().toLowerCase();
    const password = this.password;
    this.loading = true; this.error = '';
    this.auth.login({ email, password }).subscribe({
      next: () => { this.loading = false; this.router.navigate(['/hotels'], { replaceUrl: true }); },
      error: (err) => { this.loading = false; this.error = err.error?.message || 'Login failed.'; }
    });
  }

}