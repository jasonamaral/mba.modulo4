import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';

export interface MatriculaDto {
  id: string;
  alunoId: string;
  cursoId: string;
  cursoNome: string;
  dataMatricula: string;
  status: string;
  percentualConclusao: number;
  dataConclusao?: string;
}

@Injectable({ providedIn: 'root' })
export class MatriculasService extends BaseService {
  constructor(private http: HttpClient) { super(); }

  criarMatricula(cursoId: string): Observable<{ matriculaId: string }> {
    return this.http
      .post(this.UrlServiceV1 + 'alunos/matriculas', { cursoId }, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  listarMatriculas(): Observable<MatriculaDto[]> {
    return this.http
      .get(this.UrlServiceV1 + 'alunos/minhas-matriculas', this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  finalizarCurso(cursoId: string) {
    return this.http
      .post(this.UrlServiceV1 + `alunos/cursos/${cursoId}/finalizar`, {}, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  atualizarProgresso(aulaId: string, cursoId: string, percentual: number) {
    return this.http
      .post(this.UrlServiceV1 + `alunos/aulas/${aulaId}/progresso`, { cursoId, percentual }, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }
}


