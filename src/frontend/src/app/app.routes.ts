import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.authRoutes)
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.dashboardRoutes),
    canActivate: [AuthGuard]
  },
  {
    path: 'users',
    loadChildren: () => import('./features/users/users.routes').then(m => m.usersRoutes),
    canActivate: [AuthGuard]
  },
  {
    path: 'courses',
    loadChildren: () => import('./features/courses/courses.routes').then(m => m.coursesRoutes),
    canActivate: [AuthGuard]
  },
  {
    path: 'transactions',
    loadChildren: () => import('./features/transactions/transactions.routes').then(m => m.transactionsRoutes),
    canActivate: [AuthGuard]
  },
  {
    path: 'reports',
    loadChildren: () => import('./features/reports/reports.routes').then(m => m.reportsRoutes),
    canActivate: [AuthGuard]
  },
  {
    path: 'budget',
    loadChildren: () => import('./features/budget/budget.routes').then(m => m.budgetRoutes),
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    redirectTo: '/dashboard'
  }
]; 