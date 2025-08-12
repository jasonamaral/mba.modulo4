import { HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";
import { LocalStorageUtils } from "../utils/localstorage";
import { environment } from "src/environments/environment";

export abstract class BaseService {
    protected UrlServiceV1: string = environment.apiUrlv1;
    public LocalStorage = new LocalStorageUtils();

    constructor() { }

    protected getHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
    }

    protected getAuthHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Accept': 'application/json, text/plain, */*',
                'Authorization': `Bearer ${this.LocalStorage.getUserToken()}`
            })
        };
    }

    protected getAuthHeaderOnly() {
        return new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${this.LocalStorage.getUserToken()}`
        });
    }

    protected serviceError(response: Response | any) {
        let customError: string[] = [];
        let errors: string[] = [];
        let customResponse = { error: { errors: errors } }

        if (response instanceof HttpErrorResponse) {
            if (response.statusText === "Unknown Error") {
                customError.push("Ocorreu um erro desconhecido");
                response.error.errors = customError;
            }
        }

        // Tratamento específico para erros de validação (400 Bad Request)
        if (response.status === 400) {
            if (response.error && response.error.errors) {
                const validationErrors = response.error.errors;
                
                if (typeof validationErrors === 'object' && !Array.isArray(validationErrors)) {
                    Object.keys(validationErrors).forEach(field => {
                        const fieldErrors = validationErrors[field];
                        if (Array.isArray(fieldErrors)) {
                            fieldErrors.forEach((errorMessage: string) => {
                                customError.push(errorMessage);
                            });
                        }
                    });
                } else if (Array.isArray(validationErrors)) {
                    validationErrors.forEach((errorMessage: string) => {
                        customError.push(errorMessage);
                    });
                }
            } else if (response.error && response.error.title) {
                customError.push(response.error.title);
            } else {
                customError.push("Dados inválidos. Verifique as informações fornecidas.");
            }

            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 500) {
            customError.push("Ocorreu um erro no processamento, tente novamente mais tarde ou contate o nosso suporte.");
            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 404) {
            customError.push("O recurso solicitado não existe. Entre em contato com o suporte.");
            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 403) {
            customError.push("Você não tem autorização para essa ação. Faça login novamente ou contate o nosso suporte.");
            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 401) {
            this.LocalStorage.clear();
            window.location.href = '/login';
        }

        return throwError(() => response);
    }

    protected extractData(response: any): any {
        if (response && typeof response === 'object') {
            if (response.title && response.status !== undefined && response.data !== undefined) {
                return response.data || {};
            }
            
            if (response.notifications && response.notifications.length > 0) {
                // Mantém compatibilidade com estrutura antiga
            }

            if (response.result) {
                return response.result || {};
            }
        }

        return response;
    }

    protected formatDate(date: Date): string {
        return date.toISOString().split('T')[0];
    }
}


