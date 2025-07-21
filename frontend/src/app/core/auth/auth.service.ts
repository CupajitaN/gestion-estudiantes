import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';

interface LoginRequest {
  correo: string;
  contrasena: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5085/api/auth/login';
  private roleChanged = new Subject<string | null>();

  roleChanged$ = this.roleChanged.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: LoginRequest): Observable<{ token: string }> {
    return new Observable(observer => {
      this.http.post<{ token: string }>(this.apiUrl, credentials).subscribe({
        next: res => {
          localStorage.setItem('token', res.token);
          const role = this.getUserRole();
          this.roleChanged.next(role);
          observer.next(res);
          observer.complete();
        },
        error: err => observer.error(err)
      });
    });
  }

  guardarToken(token: string) {
    localStorage.setItem('token', token);
  }

  obtenerToken(): string | null {
    return localStorage.getItem('token');
  }

  eliminarToken() {
    localStorage.removeItem('token');
  }

  getUserRole(): string | null {
    const token = this.obtenerToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      return decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || null;
    } catch {
      return null;
    }
  }

  logout() {
    this.eliminarToken();
    this.router.navigate(['/auth/login']);
  }

  obtenerUsuarioActual(): any {
    const token = this.obtenerToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      return {
        id: +decoded["UserId"],
        correo: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
        rol: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
      };
    } catch (e) {
      return null;
    }
  }
}
