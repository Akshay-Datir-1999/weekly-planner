import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { RoleService } from '../services/role.service';

@Injectable({ providedIn: 'root' })
export class LeadGuard implements CanActivate {
  constructor(private roleSvc: RoleService, private router: Router) {}

  canActivate(): boolean {
    if (this.roleSvc.isLead()) return true;
    this.router.navigate(['/planning']);
    return false;
  }
}