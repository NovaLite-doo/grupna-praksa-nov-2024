import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { PieScoreComponent } from './pie-score/pie-score.component';

@Component({
  selector: 'app-exams-overview',
  templateUrl: './exams-overview.component.html',
  styleUrl: './exams-overview.component.css'
})
export class ExamsOverviewComponent {
  status: string = "All";
  exams = [1, 2, 3, 4, 5];

  changeStatusFilter(status: string) {
    this.status = status;
  }
}
