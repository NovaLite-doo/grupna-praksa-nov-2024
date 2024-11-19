import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MaslGuard } from './masl.guard';

import { PublicPageComponent } from './public-page/public-page.component';
import { RestrictedPageComponent } from './restricted-page/restricted-page.component';
import { CreateEditQuestionComponent } from './create-edit-question/create-edit-question.component';

const routes: Routes = [
  { 
    path: 'restricted-page', 
    component: RestrictedPageComponent, 
    canActivate: [MaslGuard],  
  },
  { 
    path: 'public-page', 
    component: PublicPageComponent,  
  },
  { 
    path: 'create-question', 
    component: CreateEditQuestionComponent,
    canActivate: [MaslGuard]
  },
  { 
    path: 'edit-question/:id', 
    component: CreateEditQuestionComponent,
    canActivate: [MaslGuard]
  }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
