import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../core/auth/auth.service';

@Component({
  standalone: false,
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  role: string | null = null;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.role = this.authService.getUserRole();

    this.authService.roleChanged$.subscribe(role => {
      this.role = role;
      console.log('🔄 Role actualizado:', role);
    });
  }

  logout() {
    this.authService.logout();
  }
}
