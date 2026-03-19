import { Component, inject, signal } from '@angular/core';
import { AuthService } from '@/shared/services/auth.service';
import { Router } from '@angular/router';
import { email, form, FormField, required } from '@angular/forms/signals';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ZardButtonComponent } from '@/shared/ui/components/button';
import { ZardCardComponent } from '@/shared/ui/components/card';
import { ZardCheckboxComponent } from '@/shared/ui/components/checkbox';
import {
  ZardFormControlComponent,
  ZardFormFieldComponent,
  ZardFormLabelComponent,
} from '@/shared/ui/components/form';
import { ZardInputDirective } from '@/shared/ui/components/input';

interface RegisterForm {
  email: string;
  password: string;
  repeatPassword: string;
}
@Component({
  selector: 'app-register',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    ZardButtonComponent,
    ZardCardComponent,
    ZardCheckboxComponent,
    ZardFormControlComponent,
    ZardFormFieldComponent,
    ZardFormLabelComponent,
    ZardInputDirective,
    FormField,
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  private readonly _formModel = signal<RegisterForm>({
    email: '',
    password: '',
    repeatPassword: '',
  });

  protected readonly registerForm = form(this._formModel, (schemaPath) => {
    required(schemaPath.email);
    required(schemaPath.password);
    required(schemaPath.repeatPassword);
    email(schemaPath.email);
  });

  protected readonly isLoading = signal(false);
  protected readonly errors = signal<string[]>([]);

  protected onSubmit(): void {
    const data = this.registerForm().value();
    this.authService.register(data).subscribe({
      next: () => this.router.navigate(['/login']),
      error: (err) => {
        const backendErrors = err.error?.errors || ['Registration failed'];
        this.errors.set(
          Array.isArray(backendErrors) ? backendErrors : [backendErrors]
        );
      },
    });
  }
}
