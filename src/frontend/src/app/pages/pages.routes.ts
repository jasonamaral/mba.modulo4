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
      ,
      {
        path: 'matriculas',
        loadComponent: () => import('./user/matriculas/matriculas.component').then(m => m.MatriculasComponent)
      }
      ,
      {
        path: 'certificados',
        loadComponent: () => import('./user/certificados/certificados.component').then(m => m.CertificadosComponent)
      }
      ,
      {
        path: 'cursos',
        loadComponent: () => import('./conteudo/cursos-list.component').then(m => m.CursosListComponent)
      }
      ,
      {
        path: 'cursos/:id',
        loadComponent: () => import('./conteudo/curso-detalhe.component').then(m => m.CursoDetalheComponent)
      }
    ],
  },
];
