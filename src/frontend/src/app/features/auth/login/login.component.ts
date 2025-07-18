import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="login-container">
      <mat-card class="login-card">
        <mat-card-header class="login-header">
          <mat-card-title>Plataforma Educacional</mat-card-title>
          <mat-card-subtitle>Faça login para continuar</mat-card-subtitle>
        </mat-card-header>

        <mat-card-content>
          <form [formGroup]="loginForm" (ngSubmit)="onSubmit()" class="login-form">
            
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>E-mail</mat-label>
              <input 
                matInput 
                type="email" 
                formControlName="email"
                placeholder="seu@email.com">
              <mat-icon matSuffix>email</mat-icon>
              <mat-error *ngIf="loginForm.get('email')?.hasError('required')">
                E-mail é obrigatório
              </mat-error>
              <mat-error *ngIf="loginForm.get('email')?.hasError('email')">
                Digite um e-mail válido
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Senha</mat-label>
              <input 
                matInput 
                [type]="hidePassword ? 'password' : 'text'"
                formControlName="password"
                placeholder="Digite sua senha">
              <button 
                mat-icon-button 
                matSuffix 
                type="button"
                (click)="hidePassword = !hidePassword">
                <mat-icon>{{hidePassword ? 'visibility_off' : 'visibility'}}</mat-icon>
              </button>
              <mat-error *ngIf="loginForm.get('password')?.hasError('required')">
                Senha é obrigatória
              </mat-error>
              <mat-error *ngIf="loginForm.get('password')?.hasError('minlength')">
                Senha deve ter pelo menos 6 caracteres
              </mat-error>
            </mat-form-field>

            <div class="login-actions">
              <button 
                mat-raised-button 
                color="primary" 
                type="submit"
                [disabled]="loginForm.invalid || isLoading"
                class="login-button">
                <mat-spinner *ngIf="isLoading" diameter="20"></mat-spinner>
                <span *ngIf="!isLoading">Entrar</span>
                <span *ngIf="isLoading">Entrando...</span>
              </button>
            </div>

            <div class="login-links">
              <a href="#" class="forgot-password">Esqueci minha senha</a>
            </div>

          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .login-container {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      padding: 20px;
    }

    .login-card {
      width: 100%;
      max-width: 400px;
      padding: 20px;
      box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
    }

    .login-header {
      text-align: center;
      margin-bottom: 20px;
    }

    .login-header mat-card-title {
      font-size: 1.8rem;
      color: #3f51b5;
      margin-bottom: 8px;
    }

    .login-header mat-card-subtitle {
      color: #666;
    }

    .login-form {
      display: flex;
      flex-direction: column;
    }

    .full-width {
      width: 100%;
      margin-bottom: 16px;
    }

    .login-actions {
      margin: 20px 0;
    }

    .login-button {
      width: 100%;
      height: 48px;
      font-size: 1.1rem;
    }

    .login-links {
      text-align: center;
      margin-top: 20px;
    }

    .forgot-password {
      color: #3f51b5;
      text-decoration: none;
      font-size: 0.9rem;
    }

    .forgot-password:hover {
      text-decoration: underline;
    }

    mat-spinner {
      margin-right: 8px;
    }

    @media (max-width: 480px) {
      .login-container {
        padding: 10px;
      }

      .login-card {
        padding: 15px;
      }
    }
  `]
})
export class LoginComponent {
  loginForm: FormGroup;
  hidePassword = true;
  isLoading = false;
  returnUrl: string;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

    // Get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;
      
      const credentials = this.loginForm.value;
      
      this.authService.login(credentials).subscribe({
        next: (response) => {
          this.toastr.success('Login realizado com sucesso!', 'Sucesso');
          this.router.navigate([this.returnUrl]);
        },
        error: (error) => {
          this.isLoading = false;
          let errorMessage = 'Erro ao realizar login. Tente novamente.';
          
          if (error.status === 401) {
            errorMessage = 'E-mail ou senha inválidos.';
          } else if (error.status === 0) {
            errorMessage = 'Não foi possível conectar ao servidor.';
          }
          
          this.toastr.error(errorMessage, 'Erro');
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.loginForm.controls).forEach(key => {
        this.loginForm.get(key)?.markAsTouched();
      });
    }
  }
} 