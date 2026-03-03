import { HttpInterceptorFn } from '@angular/common/http';

export const roleInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req);
};
