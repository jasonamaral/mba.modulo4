import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ConteudoListComponent } from './conteudo/conteudo-list.component';


export const PagesRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'conteudo/lista',
        component: ConteudoListComponent,
      }
    ],
  },
];
