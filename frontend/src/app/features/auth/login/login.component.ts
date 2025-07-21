import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../core/auth/auth.service';
import { TokenStorageService } from '../../../core/auth/token-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false
})
export class LoginComponent  {
  loginForm: FormGroup;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private tokenService: TokenStorageService
  ) {
    this.loginForm = this.fb.group({
      correo: ['', Validators.required],      
      contrasena: ['', Validators.required]   
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    const credenciales = this.loginForm.value;

    this.authService.login(credenciales).subscribe({
      next: (res) => {
        this.tokenService.saveToken(res.token);
        console.log('Token guardado:', res.token);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('Error al hacer login', err);
      }
    });
  }
}
