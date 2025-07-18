import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

// Dashboard DTOs
export interface DashboardAlunoDto {
  aluno: AlunoDto;
  matriculas: MatriculaDto[];
  certificados: CertificadoDto[];
  cursosRecomendados: CursoDto[];
  pagamentosRecentes: PagamentoDto[];
  progressoGeral: ProgressoGeralDto;
}

export interface DashboardAdminDto {
  estatisticasAlunos: EstatisticasAlunosDto;
  estatisticasCursos: EstatisticasCursosDto;
  relatorioVendas: RelatorioVendasDto;
  estatisticasUsuarios: EstatisticasUsuariosDto;
  cursosPopulares: CursoPopularDto[];
  vendasRecentes: VendaRecenteDto[];
}

export interface AlunoDto {
  id: string;
  nome: string;
  email: string;
  telefone: string;
  foto: string;
  dataNascimento: Date;
  cpf: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface MatriculaDto {
  id: string;
  alunoId: string;
  cursoId: string;
  cursoNome: string;
  dataMatricula: Date;
  status: string;
  percentualConclusao: number;
  dataConclusao?: Date;
  createdAt: Date;
  updatedAt: Date;
}

export interface CertificadoDto {
  id: string;
  alunoId: string;
  cursoId: string;
  cursoNome: string;
  dataEmissao: Date;
  codigoVerificacao: string;
  url: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface CursoDto {
  id: string;
  nome: string;
  descricao: string;
  categoria: string;
  preco: number;
  cargaHoraria: number;
  totalAulas: number;
  status: string;
  imagemCapa: string;
  aulas: AulaDto[];
  createdAt: Date;
  updatedAt: Date;
}

export interface AulaDto {
  id: string;
  cursoId: string;
  nome: string;
  descricao: string;
  ordem: number;
  duracaoMinutos: number;
  videoUrl: string;
  status: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface PagamentoDto {
  id: string;
  alunoId: string;
  cursoId: string;
  cursoNome: string;
  valor: number;
  status: string;
  formaPagamento: string;
  dataPagamento: Date;
  createdAt: Date;
  updatedAt: Date;
}

export interface ProgressoGeralDto {
  cursosMatriculados: number;
  cursosConcluidos: number;
  certificadosEmitidos: number;
  percentualConcluidoGeral: number;
  horasEstudadas: number;
}

export interface EstatisticasAlunosDto {
  totalAlunos: number;
  alunosAtivos: number;
  alunosInativos: number;
  novasMatriculasHoje: number;
  novasMatriculasSemana: number;
  novasMatriculasMes: number;
  taxaRetencao: number;
}

export interface EstatisticasCursosDto {
  totalCursos: number;
  cursosAtivos: number;
  cursosInativos: number;
  mediaAvaliacoes: number;
  totalAulas: number;
  horasConteudo: number;
}

export interface RelatorioVendasDto {
  vendasHoje: number;
  vendasSemana: number;
  vendasMes: number;
  vendasAno: number;
  ticketMedio: number;
  totalTransacoes: number;
  taxaConversao: number;
}

export interface EstatisticasUsuariosDto {
  totalUsuarios: number;
  usuariosAtivos: number;
  usuariosOnline: number;
  adminsAtivos: number;
  alunosAtivos: number;
}

export interface CursoPopularDto {
  id: string;
  nome: string;
  totalMatriculas: number;
  receita: number;
  mediaAvaliacoes: number;
  totalAvaliacoes: number;
}

export interface VendaRecenteDto {
  id: string;
  alunoNome: string;
  cursoNome: string;
  valor: number;
  dataVenda: Date;
  status: string;
  formaPagamento: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // Dashboard Endpoints
  getDashboardAluno(): Observable<DashboardAlunoDto> {
    return this.http.get<DashboardAlunoDto>(`${this.apiUrl}/dashboard/aluno`);
  }

  getDashboardAdmin(): Observable<DashboardAdminDto> {
    return this.http.get<DashboardAdminDto>(`${this.apiUrl}/dashboard/admin`);
  }

  // User Endpoints
  getUserProfile(): Observable<AlunoDto> {
    return this.http.get<AlunoDto>(`${this.apiUrl}/users/profile`);
  }

  updateUserProfile(data: Partial<AlunoDto>): Observable<AlunoDto> {
    return this.http.put<AlunoDto>(`${this.apiUrl}/users/profile`, data);
  }

  // Courses Endpoints
  getCursos(params?: any): Observable<CursoDto[]> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.set(key, params[key].toString());
        }
      });
    }
    return this.http.get<CursoDto[]>(`${this.apiUrl}/cursos`, { params: httpParams });
  }

  getCursoDetails(id: string): Observable<CursoDto> {
    return this.http.get<CursoDto>(`${this.apiUrl}/cursos/${id}`);
  }

  // Transactions Endpoints
  getTransacoes(params?: any): Observable<PagamentoDto[]> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.set(key, params[key].toString());
        }
      });
    }
    return this.http.get<PagamentoDto[]>(`${this.apiUrl}/transacoes`, { params: httpParams });
  }

  processarTransacao(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/transacoes/processar`, data);
  }

  // Reports Endpoints
  getRelatorioVendas(params?: any): Observable<RelatorioVendasDto> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.set(key, params[key].toString());
        }
      });
    }
    return this.http.get<RelatorioVendasDto>(`${this.apiUrl}/relatorios/vendas`, { params: httpParams });
  }

  getRelatorioAlunos(params?: any): Observable<EstatisticasAlunosDto> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.set(key, params[key].toString());
        }
      });
    }
    return this.http.get<EstatisticasAlunosDto>(`${this.apiUrl}/relatorios/alunos`, { params: httpParams });
  }
} 