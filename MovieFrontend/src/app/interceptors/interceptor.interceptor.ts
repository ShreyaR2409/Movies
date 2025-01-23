import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators'; 
import { throwError } from 'rxjs'; 

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = sessionStorage.getItem('Token');

  const modifiedReq = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      })
    : req;

  return next(modifiedReq).pipe(
    catchError((error) => {
      if (error.status === 401 || error.status === 403) {
        const router = inject(Router); 
        sessionStorage.clear(); 
        router.navigate(['/login']); 
      }
      return throwError(() => new Error(error.message));
    })
  );
};