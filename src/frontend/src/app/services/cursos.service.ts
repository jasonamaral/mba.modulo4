import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';
import { CursoCreateModel, CursoModel } from '../models/curso.model';
import { AulaCreateModel } from '../models/aula.model';

@Injectable({ providedIn: 'root' })
export class CursosService extends BaseService {
  constructor(private http: HttpClient) { super(); }

  listar(): Observable<CursoModel[]> {
    return this.http
      .get(this.UrlServiceV1 + 'Conteudos/cursos?IncludeAulas=true', this.getAuthHeaderJson())
      .pipe(
        map(r => {
          const data = this.extractData(r);
          if (!data) return [];
          // Suporta paginação: { pageSize, pageIndex, totalResults, items: [] }
          if (Array.isArray((data as any).items)) return (data as any).items as CursoModel[];
          // Suporta retorno direto como array
          if (Array.isArray(data)) return data as CursoModel[];
          // Suporta retorno de item único
          if (typeof data === 'object' && (data as any).id) return [data as CursoModel];
          return [];
        }),
        catchError(e => this.serviceError(e))
      );
  }

  obter(id: string, includeAulas = true): Observable<CursoModel> {
    return this.http
      .get(this.UrlServiceV1 + `Conteudos/${id}?includeAulas=${includeAulas}`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  create(curso: CursoCreateModel): Observable<CursoModel> {
    return this.http
      .post(this.UrlServiceV1 + 'Conteudos/cursos', curso, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  update(id: string, curso: CursoCreateModel): Observable<CursoModel> {
    return this.http
      .put(this.UrlServiceV1 + `Conteudos/cursos/${id}`, curso, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  adicionarAula(cursoId: string, aula: AulaCreateModel): Observable<any> {
    return this.http
      .post(this.UrlServiceV1 + `Conteudos/curso/${cursoId}/aula`, aula, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }
}


