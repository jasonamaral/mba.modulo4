import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';

export interface CursoDto {
  id: string;
  nome: string;
  descricao: string;
  categoriaId: string;
  valor: number;
  createdAt: string;
  updatedAt: string;
  aulas?: AulaDto[];
}

export interface AulaDto {
  id: string;
  cursoId: string;
  nome: string;
  descricao: string;
  ordem: number;
  duracaoMinutos: number;
  videoUrl: string;
}

@Injectable({ providedIn: 'root' })
export class CursosService extends BaseService {
  constructor(private http: HttpClient) { super(); }

  listar(): Observable<CursoDto[]> {
    return this.http
      .get(this.UrlServiceV1 + 'Conteudos/cursos', this.getAuthHeaderJson())
      .pipe(
        map(r => {
          const data = this.extractData(r);
          if (!data) return [];
          // Suporta paginação: { pageSize, pageIndex, totalResults, items: [] }
          if (Array.isArray((data as any).items)) return (data as any).items as CursoDto[];
          // Suporta retorno direto como array
          if (Array.isArray(data)) return data as CursoDto[];
          // Suporta retorno de item único
          if (typeof data === 'object' && (data as any).id) return [data as CursoDto];
          return [];
        }),
        catchError(e => this.serviceError(e))
      );
  }

  obter(id: string, includeAulas = true): Observable<CursoDto> {
    return this.http
      .get(this.UrlServiceV1 + `Conteudos/${id}?includeAulas=${includeAulas}`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }
}


