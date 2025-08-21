import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';
import { MatriculaCreateModel, MatriculaModel } from '../models/matricula.model';
import { SolicitarCertificadoRequest } from '../models/certificado.model';

@Injectable({ providedIn: 'root' })
export class MatriculasService extends BaseService {
  constructor(private http: HttpClient) { super(); }

  criarMatricula(data: MatriculaCreateModel): Observable<string> {
    const alunoId = this.LocalStorage.getUser()?.usuarioToken?.id;
    return this.http
      .post(this.UrlServiceV1 + `Alunos/${alunoId}/matricular-aluno`, data, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  listarMatriculas(): Observable<MatriculaModel[]> {
    return this.http
      .get(this.UrlServiceV1 + `alunos/todas-matriculas`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  finalizarCurso(cursoId: string) {
    return this.http
      .post(this.UrlServiceV1 + `alunos/cursos/${cursoId}/finalizar`, {}, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  solicitarCertificado(request: SolicitarCertificadoRequest): Observable<string> {
    const alunoId = this.LocalStorage.getUser()?.usuarioToken?.id;
    return this.http
      .post(this.UrlServiceV1 + `alunos/${alunoId}/solicitar-certificado`, request, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  atualizarProgresso(aulaId: string, cursoId: string, percentual: number) {
    return this.http
      .post(this.UrlServiceV1 + `alunos/aulas/${aulaId}/progresso`, { cursoId, percentual }, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }
}


