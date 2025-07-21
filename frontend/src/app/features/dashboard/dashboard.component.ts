import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  standalone: false,
  selector: 'app-dashboard',
  template: `
    <div class="flex">
      <app-sidebar></app-sidebar>
      <div class="flex-1 p-6">
        <router-outlet></router-outlet>
      </div>
    </div>
  `
})
export class DashboardComponent implements OnInit {
  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    const currentUrl = this.router.url;
    const role = this.authService.getUserRole();

    if (currentUrl === '/dashboard' || currentUrl === '/dashboard/') {
      if (role === 'admin' || role === 'estudiante') {
        this.router.navigate(['/dashboard/inicio']);
      } else if (role === 'profesor') {
        this.router.navigate(['/dashboard/ver-materias']);
      }
    }
  }
}
