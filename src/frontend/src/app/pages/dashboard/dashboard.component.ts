import { Component, ViewEncapsulation } from '@angular/core';
import { MaterialModule } from '../../material.module';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../services/dashboard.service';
import { DashboardAlunoModel, ProgressoGeralModel } from '../../models/dashboard-aluno.model';
import { NgApexchartsModule } from 'ng-apexcharts';
import { ApexNonAxisChartSeries, ApexChart, ApexPlotOptions, ApexStroke, ApexFill } from 'ng-apexcharts';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MaterialModule,
    CommonModule,
    NgApexchartsModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DashboardComponent {
  progressoGeral?: ProgressoGeralModel;
  carregando = false;

  // Configuração do gráfico radial (percentual concluído)
  chartSeries: ApexNonAxisChartSeries = [0];
  chartOptions: { chart: ApexChart; plotOptions: ApexPlotOptions; fill: ApexFill; stroke: ApexStroke; labels: string[] } = {
    chart: { type: 'radialBar', height: 320 },
    plotOptions: {
      radialBar: {
        hollow: { size: '60%' },
        track: { background: '#efefef' },
        dataLabels: {
          name: { show: true, color: '#666', offsetY: 12 },
          value: { show: true, fontSize: '28px', formatter: (val: number) => `${Math.round(val)}%` }
        }
      }
    },
    fill: { type: 'gradient', gradient: { shade: 'light', gradientToColors: ['#4caf50'], stops: [0, 100] } },
    stroke: { lineCap: 'round' },
    labels: ['Concluído']
  };

  constructor(private dashboard: DashboardService) { }

  ngOnInit() {
    this.carregando = true;
    this.dashboard.getDashboardAluno().subscribe({
      next: (data: DashboardAlunoModel) => {
        this.progressoGeral = data?.progressoGeral ?? {
          cursosMatriculados: 0,
          cursosConcluidos: 0,
          certificadosEmitidos: 0,
          percentualConcluidoGeral: 0,
          horasEstudadas: 0
        };

        const percentual = Number(this.progressoGeral.percentualConcluidoGeral ?? 0);
        this.chartSeries = [isNaN(percentual) ? 0 : Math.max(0, Math.min(100, percentual))];
      },
      error: () => {
        this.progressoGeral = {
          cursosMatriculados: 0,
          cursosConcluidos: 0,
          certificadosEmitidos: 0,
          percentualConcluidoGeral: 0,
          horasEstudadas: 0
        };
        this.chartSeries = [0];
      },
      complete: () => this.carregando = false
    });
  }
}
