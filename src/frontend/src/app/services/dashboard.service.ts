import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';
type CardSumaryModel = any;
type CategoryTransactionGraphModel = any[];
type TransactionYearEvolutionGraphModel = any[];

@Injectable({ providedIn: 'root' })
export class DashboardService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getCardSumary(filterDate: Date | null): Observable<CardSumaryModel> {
    let url: string = `${this.UrlServiceV1}v1/dashboard/cards/`;
    if (filterDate) {
      url += this.formatDate(filterDate);
    }

    let response = this.http
      .get(url, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  getTransactionCategorySumary(filterDate: Date | null): Observable<CategoryTransactionGraphModel[]> {
    let url: string = `${this.UrlServiceV1}v1/dashboard/transactions/`;
    if (filterDate) {
      url += this.formatDate(filterDate);
    }

    let response = this.http
      .get(url, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  getTransactionInYearEvolution(filterDate: Date | null): Observable<TransactionYearEvolutionGraphModel[]> {
    let url: string = `${this.UrlServiceV1}v1/dashboard/evolution/`;
    if (filterDate){
      url += this.formatDate(filterDate);
    }

    let response = this.http
      .get(url, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }
}
