import { Injectable } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class GlobalErrorInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: any) => {
        if (error instanceof HttpErrorResponse) {
          // Pula erros 401 pois já há tratamento que redireciona no BaseService
          if (error.status === 401) {
            return throwError(() => error);
          }

          const extracted = this.extractErrors(error);
          if (extracted && extracted.length > 0) {
            this.toastr.error(extracted.join('\n'));
          } else {
            this.toastr.error('Ocorreu um erro inesperado. Tente novamente.');
          }
        } else {
          this.toastr.error('Erro de comunicação. Verifique sua conexão.');
        }

        return throwError(() => error);
      })
    );
  }

  private extractErrors(error: HttpErrorResponse): string[] {
    const errors = (error?.error?.errors ?? []) as string[];
    if (Array.isArray(errors) && errors.length > 0) return errors;
    const title = (error?.error?.title ?? error?.message ?? '') as string;
    return title ? [title] : [];
  }
}


