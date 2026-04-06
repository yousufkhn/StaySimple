import { HttpInterceptorFn } from '@angular/common/http';
import { Auth } from '../services/auth/auth';
import { inject } from '@angular/core';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Keep public endpoints simple to avoid unnecessary preflight/auth coupling.
  const isAuthEndpoint = req.url.includes('/api/auth/');
  const isPublicHotelGet = req.method === 'GET' && req.url.includes('/api/hotels');
  if (isAuthEndpoint || isPublicHotelGet) {
    return next(req);
  }

  const token = inject(Auth).getToken();
  if(token){
    return next(req.clone({setHeaders : {Authorization : `Bearer ${token}`}}));
  }
  return next(req);
};
