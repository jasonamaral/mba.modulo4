import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';

export interface CertificadoDto {
  id: string;
  alunoId: string;
  cursoId: string;
  cursoNome: string;
  dataEmissao: string;
  codigoVerificacao: string;
  url: string;
}

@Injectable({ providedIn: 'root' })
export class CertificadosService extends BaseService {
  constructor(private http: HttpClient) { super(); }

  listar(): Observable<CertificadoDto[]> {
    return this.http
      .get(this.UrlServiceV1 + 'alunos/certificados', this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }
}


