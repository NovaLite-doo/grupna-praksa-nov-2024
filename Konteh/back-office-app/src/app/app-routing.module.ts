import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PublicPageComponent } from './public-page/public-page.component';
import { RestrictedPageComponent } from './restricted-page/restricted-page.component';
import { MaslGuard } from './masl.guard';

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
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
