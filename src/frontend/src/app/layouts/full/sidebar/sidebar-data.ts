import { NavItem } from './nav-item/nav-item';

export const navItems: NavItem[] = [
  {
    navCap: 'Dashboard',
  },
  {
    displayName: 'Dashboard',
    iconName: 'material-symbols-light:dashboard-outline',
    route: 'pages/dashboard',
  },
  {
    navCap: 'Conteúdo',
    divider: true
  },
  {
    displayName: 'Manutenção',
    iconName: 'material-symbols-light:engineering-outline',
    route: 'pages/conteudo/lista',
  },
  {
    navCap: 'Area do Aluno',
    divider: true
  },
  {
    displayName: 'Meus cursos',
    iconName: 'fluent:money-hand-16-regular',
    route: 'pages/aluno/cursos',
  }
];
