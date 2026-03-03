import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { PlanEntry, WeeklyPlan, DashboardEntry } from '../models';

@Injectable({ providedIn: 'root' })
export class PlanService {
  private api = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getPlan(memberId: number, weekCycleId: number) {
    return this.http.get<WeeklyPlan>(`${this.api}/plan/${memberId}/${weekCycleId}`);
  }
  submitPlan(memberId: number, weekCycleId: number, entries: PlanEntry[]) {
    return this.http.post<WeeklyPlan>(`${this.api}/plan/submit`, { memberId, weekCycleId, entries });
  }
  updateProgress(entryId: number, progressPercent: number, actualHours?: number) {
    return this.http.put(`${this.api}/plan/progress/${entryId}`, { progressPercent, actualHours });
  }
  getDashboard(weekCycleId: number) {
    return this.http.get<DashboardEntry[]>(`${this.api}/plan/week/${weekCycleId}/dashboard`);
  }
}
