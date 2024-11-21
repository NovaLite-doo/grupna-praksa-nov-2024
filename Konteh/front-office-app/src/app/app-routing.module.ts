import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { CandidateFormComponent } from './candidate-form/candidate-form.component';
import { ExamOverviewComponent } from './exam-overview/exam-overview.component';

const routes: Routes = [
  { path: 'candidate-form', component: CandidateFormComponent },
  { path: 'exam-overview/:id', component: ExamOverviewComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

 }
