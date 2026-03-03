import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { Member } from '../../models';
import { RoleService } from '../../services/role.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule,
    MatToolbarModule, MatButtonModule, MatSelectModule, MatIconModule],
  templateUrl: './navbar.component.html'
})
export class NavbarComponent implements OnInit {
  members: Member[] = [];
  selected!: Member;
  isDark = false;

  constructor(public roleSvc: RoleService, public router: Router) {}

  ngOnInit() {
    this.members = this.roleSvc.getMembers();
    this.selected = this.roleSvc.getCurrent();
    this.roleSvc.currentMember$.subscribe((m: Member) => { this.selected = m; });
  }

  onSwitch(m: Member) { this.roleSvc.switchTo(m); this.router.navigate(['/planning']); }
  toggleDark() { this.isDark = !this.isDark; document.body.classList.toggle('dark-theme'); }
  isLeadRoute() { return this.roleSvc.isLead(); }
}