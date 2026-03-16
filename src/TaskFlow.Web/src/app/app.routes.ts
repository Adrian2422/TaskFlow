import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./features/landing/landing').then((c) => c.Landing),
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard').then((c) => c.Dashboard),
  },
  {
    path: '**',
    redirectTo: '',
  },
];
