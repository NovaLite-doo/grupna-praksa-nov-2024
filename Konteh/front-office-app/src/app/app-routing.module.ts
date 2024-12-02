import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidateFormComponent } from './candidate-form/candidate-form.component';
import { ExamOverviewComponent } from './exam-overview/exam-overview.component';
import { ThankYouPageComponent } from './thank-you-page/thank-you-page.component';

const routes: Routes = [
  { path: '', redirectTo: '/candidate-form', pathMatch: 'full' },
  { path: 'candidate-form', component: CandidateFormComponent },
  { path: 'exam-overview/:id', component: ExamOverviewComponent },
  { path: 'thank-you', component: ThankYouPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

 }
