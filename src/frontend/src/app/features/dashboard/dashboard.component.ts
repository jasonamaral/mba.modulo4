import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { NgApexchartsModule } from 'ng-apexcharts';
import { Observable } from 'rxjs';
import { ApiService, DashboardAlunoDto, DashboardAdminDto } from '../../core/services/api.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatGridListModule,
    MatProgressBarModule,
    NgApexchartsModule
  ],
  template: `
    <div class="dashboard-container">
      <h1 class="dashboard-title">Dashboard</h1>
      
      <!-- Dashboard do Aluno -->
      <div *ngIf="isAluno" class="dashboard-content">
        <div class="stats-grid">
          <mat-card class="stat-card">
            <mat-card-header>
              <mat-card-title>Cursos Matriculados</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <h2>{{ dashboardData?.progressoGeral?.cursosMatriculados || 0 }}</h2>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card">
            <mat-card-header>
              <mat-card-title>Cursos Concluídos</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <h2>{{ dashboardData?.progressoGeral?.cursosConcluidos || 0 }}</h2>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card">
            <mat-card-header>
              <mat-card-title>Certificados</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <h2>{{ dashboardData?.progressoGeral?.certificadosEmitidos || 0 }}</h2>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card">
            <mat-card-header>
              <mat-card-title>Horas Estudadas</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <h2>{{ dashboardData?.progressoGeral?.horasEstudadas || 0 }}</h2>
            </mat-card-content>
          </mat-card>
        </div>

        <div class="charts-section">
          <mat-card class="chart-card">
            <mat-card-header>
              <mat-card-title>Progresso Geral</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <mat-progress-bar 
                mode="determinate" 
                [value]="dashboardData?.progressoGeral?.percentualConcluidoGeral || 0">
              </mat-progress-bar>
              <p>{{ dashboardData?.progressoGeral?.percentualConcluidoGeral || 0 }}% concluído</p>
            </mat-card-content>
          </mat-card>
        </div>

        <div class="recent-section">
          <mat-card class="recent-card">
            <mat-card-header>
              <mat-card-title>Matrículas Recentes</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <div *ngFor="let matricula of dashboardData?.matriculas" class="recent-item">
                <h4>{{ matricula.cursoNome }}</h4>
                <p>{{ matricula.dataMatricula | date:'dd/MM/yyyy' }}</p>
                <mat-progress-bar 
                  mode="determinate" 
                  [value]="matricula.percentualConclusao">
                </mat-progress-bar>
              </div>
            </mat-card-content>
          </mat-card>
        </div>
      </div>

      <!-- Dashboard do Admin -->
      <div *ngIf="isAdmin" class="dashboard-content">
        <div class="admin-stats-grid">
          <mat-card class="stat-card">
            <mat-card-header>
              <mat-card-title>Total Alunos</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <h2>{{ adminData?.estatisticasAlunos?.totalAlunos || 0 }}</h2>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card">
            <mat-card-header>
              <mat-card-title>Total Cursos</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <h2>{{ adminData?.estatisticasCursos?.totalCursos || 0 }}</h2>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card">
            <mat-card-header>
              <mat-card-title>Vendas do Mês</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <h2>{{ adminData?.relatorioVendas?.vendasMes | currency:'BRL':'symbol':'1.2-2' }}</h2>
            </mat-card-content>
          </mat-card>

          <mat-card class="stat-card">
            <mat-card-header>
              <mat-card-title>Usuários Online</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <h2>{{ adminData?.estatisticasUsuarios?.usuariosOnline || 0 }}</h2>
            </mat-card-content>
          </mat-card>
        </div>
      </div>

      <!-- Loading State -->
      <div *ngIf="isLoading" class="loading-container">
        <mat-progress-bar mode="indeterminate"></mat-progress-bar>
        <p>Carregando dados...</p>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-container {
      padding: 20px;
    }

    .dashboard-title {
      margin-bottom: 30px;
      color: #333;
    }

    .stats-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 20px;
      margin-bottom: 30px;
    }

    .admin-stats-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 20px;
      margin-bottom: 30px;
    }

    .stat-card {
      text-align: center;
      padding: 20px;
    }

    .stat-card h2 {
      font-size: 2.5rem;
      color: #3f51b5;
      margin: 10px 0;
    }

    .charts-section {
      margin-bottom: 30px;
    }

    .chart-card {
      padding: 20px;
    }

    .recent-section {
      margin-bottom: 30px;
    }

    .recent-card {
      padding: 20px;
    }

    .recent-item {
      margin-bottom: 15px;
      padding: 10px;
      border-left: 4px solid #3f51b5;
    }

    .recent-item h4 {
      margin: 0 0 5px 0;
      color: #333;
    }

    .recent-item p {
      margin: 0 0 10px 0;
      color: #666;
      font-size: 0.9rem;
    }

    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 50px;
    }

    .loading-container p {
      margin-top: 20px;
      color: #666;
    }

    @media (max-width: 768px) {
      .dashboard-container {
        padding: 10px;
      }

      .stats-grid {
        grid-template-columns: 1fr;
      }

      .admin-stats-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class DashboardComponent implements OnInit {
  isLoading = false;
  dashboardData: DashboardAlunoDto | null = null;
  adminData: DashboardAdminDto | null = null;
  isAluno = false;
  isAdmin = false;

  constructor(
    private apiService: ApiService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.checkUserRole();
    this.loadDashboardData();
  }

  private checkUserRole(): void {
    this.isAluno = this.authService.hasRole('Aluno');
    this.isAdmin = this.authService.hasRole('Admin');
  }

  private loadDashboardData(): void {
    this.isLoading = true;

    if (this.isAluno) {
      this.apiService.getDashboardAluno().subscribe({
        next: (data) => {
          this.dashboardData = data;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar dashboard do aluno:', error);
          this.isLoading = false;
        }
      });
    } else if (this.isAdmin) {
      this.apiService.getDashboardAdmin().subscribe({
        next: (data) => {
          this.adminData = data;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar dashboard do admin:', error);
          this.isLoading = false;
        }
      });
    } else {
      this.isLoading = false;
    }
  }
} 