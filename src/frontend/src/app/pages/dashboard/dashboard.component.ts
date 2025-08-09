import { Component, ViewEncapsulation } from '@angular/core';
import { MaterialModule } from '../../material.module';
import { DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MaterialModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DashboardComponent {
  progressoGeral?: { percentualConcluidoGeral: number, cursosMatriculados: number };
  constructor(private dashboard: DashboardService) { }
  ngOnInit() {
    // TODO: trocar para endpoint do BFF de dashboard quando mapeado no service
    // Mantém compatibilidade mínima para exibição do layout
    this.progressoGeral = { percentualConcluidoGeral: 0, cursosMatriculados: 0 };
  }
}
