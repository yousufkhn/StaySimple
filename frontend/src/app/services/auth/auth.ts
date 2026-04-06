import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../models/models';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environments';

// tells angular that this service can be injected into other components or services, and it will be provided in the root injector, making it a singleton throughout the application.
@Injectable({
  // This means that Angular will create a single instance of the Auth service and share it across the entire application. This is useful for services that manage state or perform operations that should be consistent across the app, such as authentication.
  providedIn: 'root',
})
export class Auth {
  private baseUrl = environment.apiBaseUrl + '/auth';
  private userKey = "stayeasy_user";

  // this is bascially a way to store the current user's authentication state. It uses a BehaviorSubject, which is a type of Observable that holds a value and emits it to subscribers whenever it changes. Initially, it is set to null, indicating that there is no authenticated user. When a user logs in successfully, this subject can be updated with the user's authentication information, allowing other parts of the application to react to changes in the authentication state.
  private currentUserSubject = new BehaviorSubject<AuthResponse | null>(null);
  // This is an Observable that components can subscribe to in order to get updates about the current user's authentication state. Whenever the currentUserSubject is updated (e.g., when a user logs in or out), any component subscribed to currentUser$ will receive the new value, allowing it to react accordingly (e.g., showing user-specific content or hiding login options).
  currentUser$ = this.currentUserSubject.asObservable();
  
  // this constructor is responsible for initializing the Auth service. It checks if there is a stored user in the browser's local storage under the key "stayeasy_user". If it finds a stored user, it parses the JSON string and updates the currentUserSubject with that user's information. This allows the application to maintain the user's authentication state even after a page refresh or when the user returns to the app later, as long as the local storage entry exists.
  constructor(private http: HttpClient) {
    const stored = localStorage.getItem(this.userKey);
    if (stored) {
      try {
        const parsed = JSON.parse(stored);
        const normalized = this.normalizeAuthResponse(parsed);
        if (!normalized.token) {
          localStorage.removeItem(this.userKey);
          this.currentUserSubject.next(null);
        } else {
          this.currentUserSubject.next(normalized);
        }
      } catch {
        localStorage.removeItem(this.userKey);
      }
    }
  }
 
  register(data : RegisterRequest) : Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${this.baseUrl}/register`,data)
    .pipe(tap(res => this.setUser(res)));
  }

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, data)
      .pipe(tap(res => this.setUser(res)));
  }

  logout(): void {
    localStorage.removeItem('stayeasy_user');
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    const user = this.currentUserSubject.value as any;
    return user?.token ?? user?.Token ?? null;
  }
  isLoggedIn(): boolean { return !!this.getToken(); }
  isAdmin(): boolean { return this.currentUserSubject.value?.role === 'Admin'; }
  getUserName(): string { return this.currentUserSubject.value?.name ?? ''; }

  private setUser(res: AuthResponse): void {
    const normalized = this.normalizeAuthResponse(res as any);
    if (!normalized.token) {
      localStorage.removeItem(this.userKey);
      this.currentUserSubject.next(null);
      return;
    }
    localStorage.setItem('stayeasy_user', JSON.stringify(normalized));
    this.currentUserSubject.next(normalized);
  }

  private normalizeAuthResponse(res: any): AuthResponse {
    return {
      token: res?.token ?? res?.Token ?? '',
      name: res?.name ?? res?.Name ?? '',
      email: res?.email ?? res?.Email ?? '',
      role: res?.role ?? res?.Role ?? ''
    };
  }
}
