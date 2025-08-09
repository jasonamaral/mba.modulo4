import { Component, ViewEncapsulation } from '@angular/core';
import { MaterialModule } from '../../material.module';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MaterialModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DashboardComponent { }
