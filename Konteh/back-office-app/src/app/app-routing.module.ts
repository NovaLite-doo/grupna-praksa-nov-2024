import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MaslGuard } from './masl.guard';


const routes: Routes = [
  {
    path: '',
    redirectTo: 'questions',
    pathMatch: 'full'
  },
  {
    path: 'questions',
    loadChildren: () => import('./features/questions/questions.module').then(m => m.QuestionsModule),
    canActivate: [MaslGuard]
  }
  ,
  {
    path: 'exams',
    loadChildren: () => import('./features/exams/exams.module').then(m => m.ExamsModule),
    canActivate: [MaslGuard]
  }
]


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
