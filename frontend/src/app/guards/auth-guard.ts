import { CanActivateFn, Router } from '@angular/router';
import { Auth } from '../services/auth/auth';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = () => {
  const auth = inject(Auth);
  const router = inject(Router);

  if (auth.isLoggedIn()) return true;
  return router.createUrlTree(['/login']);
};

export const publicOnlyGuard: CanActivateFn = () => {
  const auth = inject(Auth);
  const router = inject(Router);

  if (!auth.isLoggedIn()) return true;
  return router.createUrlTree([auth.isAdmin() ? '/admin/manage-hotels' : '/hotels']);
};

export const adminGuard: CanActivateFn = () => {
  const auth = inject(Auth);
  const router = inject(Router);

  if (auth.isLoggedIn() && auth.isAdmin()) return true;
  return router.createUrlTree(['/']);
}

export const userGuard: CanActivateFn = () => {
  const auth = inject(Auth);
  const router = inject(Router);

  if (auth.isLoggedIn() && !auth.isAdmin()) return true;
  return router.createUrlTree([auth.isAdmin() ? '/admin/manage-hotels' : '/login']);
};


