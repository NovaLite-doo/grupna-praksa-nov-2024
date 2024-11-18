import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateEditQuestionComponent } from './create-edit-question/create-edit-question.component';

const routes: Routes = [
  { path: 'create-question', component: CreateEditQuestionComponent },
  { path: 'edit-question/:id', component: CreateEditQuestionComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
