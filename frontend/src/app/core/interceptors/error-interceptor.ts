import { Injectable } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

export enum STATUS {
  UNAUTHORIZED = 401,
  FORBIDDEN = 403,
  NOT_FOUND = 404,
  INTERNAL_SERVER_ERROR = 500,
}

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private errorPages = [STATUS.FORBIDDEN, STATUS.NOT_FOUND, STATUS.INTERNAL_SERVER_ERROR];

  constructor(private router: Router, private toast: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next
      .handle(request)
      .pipe(catchError((error: HttpErrorResponse) => this.handleError(error)));
  }

  private getMessage = (error: HttpErrorResponse) => {
    if (error.error?.message) {
      return error.error.message;
    }

    if (error.error?.error) {
      return error.error?.error;
    }

    if (error.error?.errors) {
      return Object.values(error.error?.errors)[0];
    }

    if (error.error?.msg) {
      return error.error.msg;
    }

    return `${error.status} ${error.statusText}`;
  };

  private handleError(error: HttpErrorResponse) {
    if (this.errorPages.includes(error.status)) {
      this.router.navigateByUrl(`/${error.status}`, {
        skipLocationChange: true,
      });
    } else {
      this.toast.error(this.getMessage(error));
      if (error.status === STATUS.UNAUTHORIZED) {
        this.router.navigateByUrl('/auth/login');
      }
    }

    return throwError(error);
  }
}
