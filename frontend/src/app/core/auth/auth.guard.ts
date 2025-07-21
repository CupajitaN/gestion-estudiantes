import { Injectable } from '@angular/core';
import {
  CanActivate, Router
} from '@angular/router';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private tokenService: TokenStorageService, private router: Router) {}

  canActivate(): boolean {
    const token = this.tokenService.getToken();
    if (token) {
      return true;
    }

    this.router.navigate(['/auth/login']);
    return false;
  }
}
