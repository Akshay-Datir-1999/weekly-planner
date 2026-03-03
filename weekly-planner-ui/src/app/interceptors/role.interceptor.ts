import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RoleService } from '../services/role.service';

@Injectable()
export class RoleInterceptor implements HttpInterceptor {
  constructor(private roleSvc: RoleService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const m = this.roleSvc.getCurrent();
    return next.handle(req.clone({
      setHeaders: {
        'X-Member-Id': m.id.toString(),
        'X-Is-Lead': m.isLead.toString()
      }
    }));
  }
}