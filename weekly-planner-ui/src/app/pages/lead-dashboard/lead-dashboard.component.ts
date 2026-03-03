import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { DashboardEntry, WeekCycle } from '../../models';
import { PlanService } from '../../services/plan.service';
import { WeekCycleService } from '../../services/week-cycle.service';

@Component({
  selector: 'app-lead-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule,
    MatProgressBarModule, MatProgressSpinnerModule,
    MatFormFieldModule, MatSelectModule,
    MatExpansionModule, MatTableModule, MatChipsModule],
  templateUrl: './lead-dashboard.component.html'
})
export class LeadDashboardComponent implements OnInit {
  dashboard: DashboardEntry[] = [];
  allWeeks: WeekCycle[] = [];
  selectedWeekId?: number;
  isLoading = true;
  errorMessage = '';
  cols = ['title','category','plannedHours','progressPercent','actualHours'];
  catLabel = (c: number) => (['','Client Focused','Tech Debt','R&D'][c] ?? '');

  constructor(private planSvc: PlanService, private weekSvc: WeekCycleService) {}

  ngOnInit() {
    this.weekSvc.getAll().subscribe({
      next: weeks => {
        this.allWeeks = weeks;
        if (weeks.length > 0) { this.selectedWeekId = weeks[0].id; this.loadDashboard(weeks[0].id); }
        else { this.isLoading = false; }
      },
      error: () => { this.errorMessage = 'Failed to load weeks.'; this.isLoading = false; }
    });
  }

  onWeekChange(weekId: number) { this.selectedWeekId = weekId; this.loadDashboard(weekId); }

  loadDashboard(weekId: number) {
    this.isLoading = true;
    this.planSvc.getDashboard(weekId).subscribe({
      next: data => { this.dashboard = data; this.isLoading = false; },
      error: () => { this.errorMessage = 'Failed to load dashboard.'; this.isLoading = false; }
    });
  }
}
