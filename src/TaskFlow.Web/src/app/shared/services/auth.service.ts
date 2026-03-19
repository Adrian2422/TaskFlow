import { inject, Injectable, signal } from '@angular/core';
import { AuthService as AuthApiService } from '@/shared/api/auth/auth.service';
import { Observable, tap } from 'rxjs';
import {
  AuthResponseDto,
  LoginRequest,
  RegisterRequest,
  RegisterResponseDto,
} from '@/shared/api/model';

export interface AuthResponse {
  token: string;
}

export interface User {
  id: string;
  email: string;
  fullName: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly authApiService = inject(AuthApiService);

  currentUser = signal<User | null>(this.getUserFromToken());

  login(credentials: LoginRequest): Observable<AuthResponseDto> {
    return this.authApiService.login(credentials).pipe(
      tap((response) => {
        localStorage.setItem('auth_token', response.token!);
        this.currentUser.set(this.getUserFromToken());
      })
    );
  }

  register(userData: RegisterRequest): Observable<RegisterResponseDto> {
    return this.authApiService.register(userData);
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    this.currentUser.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  private getUserFromToken(): User | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return {
        id: payload[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ],
        email:
          payload[
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
          ],
        fullName: payload['fullName'],
      };
    } catch (e) {
      return null;
    }
  }
}
