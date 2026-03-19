import { Component, inject, signal } from '@angular/core';
import { AuthService } from '@/shared/services/auth.service';
import { Router } from '@angular/router';
import { email, form, FormField, required } from '@angular/forms/signals';
import { ZardCardComponent } from '@/shared/ui/components/card';
import { FormsModule } from '@angular/forms';
import { ZardFormImports } from '@/shared/ui/components/form';
import { ZardInputDirective } from '@/shared/ui/components/input';
import { ZardButtonComponent } from '@/shared/ui/components/button';
import { ZardCheckboxComponent } from '@/shared/ui/components/checkbox';

interface LoginForm {
  email: string;
  password: string;
  rememberMe: boolean;
}

@Component({
  selector: 'app-login',
  imports: [
    ZardCardComponent,
    FormsModule,
    ZardFormImports,
    ZardInputDirective,
    ZardButtonComponent,
    ZardCheckboxComponent,
    FormField,
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  private readonly _formModel = signal<LoginForm>({
    email: '',
    password: '',
    rememberMe: false,
  });

  protected readonly loginForm = form(this._formModel, (schemaPath) => {
    required(schemaPath.email);
    required(schemaPath.password);
    email(schemaPath.email);
  });

  protected readonly isLoading = signal(false);
  protected readonly error = signal<string | null>(null);

  protected onSubmit() {
    const data = this.loginForm().value();
    this.authService.login(data).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err) => this.error.set(err.error?.error || 'Login failed'),
    });
  }
}
