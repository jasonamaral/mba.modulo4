import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { CategoryModel } from '../models/conteudo.model';

@Injectable({ providedIn: 'root' })
export class ConteudoService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getAll(): Observable<CategoryModel[]> {
    let response = this.http
      .get(this.UrlServiceV1 + 'conteudo', this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  // Novos endpoints para categorias conforme API /Conteudos/categorias
  getAllCategories(): Observable<CategoryModel[]> {
    return this.http
      .get(this.UrlServiceV1 + 'Conteudos/categorias', this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  create(category: CategoryModel): Observable<CategoryModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'conteudo', category, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  createCategory(category: CategoryModel): Observable<CategoryModel> {
    return this.http
      .post(this.UrlServiceV1 + 'Conteudos/categorias', category, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  update(category: CategoryModel): Observable<CategoryModel> {
    let response = this.http
      .put(this.UrlServiceV1 + 'conteudo/' + category.categoryId, category, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  updateCategory(category: CategoryModel): Observable<CategoryModel> {
    return this.http
      .put(this.UrlServiceV1 + 'Conteudos/categorias/' + category.categoryId, category, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

  delete(categoryId: string): Observable<void> {
    let response = this.http
      .delete(this.UrlServiceV1 + `conteudo/${categoryId}`, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  deleteCategory(categoryId: string): Observable<void> {
    return this.http
      .delete(this.UrlServiceV1 + `Conteudos/categorias/${categoryId}`, this.getAuthHeaderJson())
      .pipe(map(r => this.extractData(r)), catchError(e => this.serviceError(e)));
  }

}