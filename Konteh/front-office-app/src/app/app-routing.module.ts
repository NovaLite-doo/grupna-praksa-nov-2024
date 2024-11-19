import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ExamOverviewComponent } from './features/exam-overview/exam-overview.component';

const routes: Routes = [
  { path: 'exam-overview', component: ExamOverviewComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

 }
