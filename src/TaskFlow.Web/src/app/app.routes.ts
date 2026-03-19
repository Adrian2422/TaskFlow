import { Routes } from '@angular/router';
import { authGuard } from '@/shared/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./features/landing/landing').then((c) => c.Landing),
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login').then((c) => c.Login),
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register').then((c) => c.Register),
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard').then((c) => c.Dashboard),
    canActivate: [authGuard],
  },
  {
    path: '**',
    redirectTo: '',
  },
];
