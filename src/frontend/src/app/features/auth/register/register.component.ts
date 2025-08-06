import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';
import { NgxMaskDirective } from 'ngx-mask';
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MatMomentDateModule } from '@angular/material-moment-adapter';

export const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatMomentDateModule,
    NgxMaskDirective,
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'pt-br' },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS }
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registerForm: FormGroup;
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
    this.registerForm = this.fb.group({
      nome: ['Pedro', [Validators.required, Validators.maxLength(100)]],
      email: ['pedro@gmail.com', [Validators.required, Validators.email]],
      senha: ['123456', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      dataNascimento: [new Date(), [Validators.required]],
      cPF: ['41037966856', [Validators.required]],
      telefone: [''],
      genero: ['', [Validators.maxLength(20)]],
      cidade: ['', [Validators.maxLength(100)]],
      estado: ['', [Validators.maxLength(50)]],
      cEP: ['', [Validators.maxLength(10)]],
    });

    // Get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.isLoading = true;

      const credentials = this.registerForm.value;

      this.authService.register(credentials).subscribe({
        next: (response) => {
          this.toastr.success('Registro realizado com sucesso!', 'Sucesso');
          this.router.navigate([this.returnUrl]);
        },
        error: (error) => {
          this.isLoading = false;
          let errorMessage = 'Erro ao realizar registro. Tente novamente.';

          if (error.status === 0) {
            errorMessage = 'Não foi possível conectar ao servidor.';
          }

          if (error?.error) {
            this.applyBackendErrors(error.error);
          }

          this.toastr.error(errorMessage, 'Erro');
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.registerForm.controls).forEach(key => {
        this.registerForm.get(key)?.markAsTouched();
      });
    }
  }

  generalErrors: string[] = [];

  applyBackendErrors(errorResponse: any) {
    this.generalErrors = [];

    if (errorResponse.errors) {
      Object.keys(errorResponse.errors).forEach(field => {
        const formControl = this.registerForm.get(this.mapFieldName(field));
        const messages = errorResponse.errors[field];

        if (formControl) {
          formControl.setErrors({ serverError: messages.join(' ') });
        } else {
          this.generalErrors.push(...messages);
        }
      });
    }

    if (errorResponse.details && Array.isArray(errorResponse.details)) {
      this.generalErrors.push(...errorResponse.details);
    } else if (errorResponse.message) {
      this.generalErrors.push(errorResponse.message);
    }
  }

  // applyServerErrors(serverErrors: Record<string, string[]>) {
  //   this.generalErrors = [];

  //   Object.keys(serverErrors).forEach(field => {
  //     const formControl = this.registerForm.get(this.mapFieldName(field));
  //     const messages = serverErrors[field];

  //     if (formControl) {
  //       // Setar erro personalizado no FormControl
  //       formControl.setErrors({ serverError: messages.join(' ') });
  //     } else {
  //       // Campo não encontrado no formulário, adiciona na lista geral
  //       this.generalErrors.push(...messages);
  //     }
  //   });
  // }

  // Se o nome dos campos do backend for diferente do formulário, ajuste aqui:
  mapFieldName(serverFieldName: string): string {
    return serverFieldName[0].toLowerCase() + serverFieldName.slice(1);
  }
} 