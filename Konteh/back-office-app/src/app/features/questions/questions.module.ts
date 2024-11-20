import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { QuestionsOverviewComponent } from './questions-overview/questions-overview.component';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { HttpClientModule } from '@angular/common/http';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CreateEditQuestionComponent } from './create-edit-question/create-edit-question.component';
import { QuestionFormComponent } from './create-edit-question/question-form/question-form.component';
import { AnswerFormComponent } from './create-edit-question/answer-form/answer-form.component';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { QuestionsRoutingModule } from './questions-routing.module';




@NgModule({
  declarations: [
    QuestionsOverviewComponent,
    CreateEditQuestionComponent,
    QuestionFormComponent,
    AnswerFormComponent
  ],
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    QuestionsRoutingModule,
    MatSortModule,
    HttpClientModule,
    MatMenuModule,
    MatIconModule,
    MatDialogModule,
    MatSelectModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCheckboxModule
  ]
})
export class QuestionsModule { }
