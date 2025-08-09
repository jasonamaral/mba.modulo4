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
      .pipe(map(r => this.extractData(r)?.items ?? this.extractData(r) ?? []), catchError(e => this.serviceError(e)));
  }

  obter(id: string, includeAulas = true): Observable<CursoDto> {
    return this.http
      .get(this.UrlServiceV1 + `Conteudos/${id}?includeAulas=${includeAulas}`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }
}


