import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  template: `
    <app-navbar></app-navbar>
    <div style="padding: 20px">
      <router-outlet></router-outlet>
    </div>
  `
})
export class AppComponent {
  title = 'weekly-planner-ui';
}