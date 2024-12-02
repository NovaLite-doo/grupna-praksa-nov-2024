import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExamsOverviewComponent } from "./exams-overview/exams-overview.component";
import { ExamsRoutingModule } from './exams-routing-module';
import { MatTableModule } from '@angular/material/table';
import {MatCardModule} from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { ExamCardComponent } from './exams-overview/exam-card/exam-card.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    ExamsOverviewComponent,
    ExamCardComponent
  ],
  imports: [
    CommonModule,
    ExamsRoutingModule,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class ExamsModule { }
