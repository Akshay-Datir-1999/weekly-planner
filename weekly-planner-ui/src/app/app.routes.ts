import { Routes } from '@angular/router';
import { BacklogComponent } from './pages/backlog/backlog.component';
import { PlanningComponent } from './pages/planning/planning.component';
import { LeadDashboardComponent } from './pages/lead-dashboard/lead-dashboard.component';
import { WeekManagerComponent } from './pages/week-manager/week-manager.component';
import { LeadGuard } from './guards/lead-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'backlog', pathMatch: 'full' },
  { path: 'backlog', component: BacklogComponent },
  { path: 'planning', component: PlanningComponent },
  { path: 'dashboard', component: LeadDashboardComponent, canActivate: [LeadGuard] },
  { path: 'weeks', component: WeekManagerComponent, canActivate: [LeadGuard] },
];