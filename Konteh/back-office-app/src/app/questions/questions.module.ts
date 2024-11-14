import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';  
import { QuestionsOverviewComponent } from './questions-overview/questions-overview.component';  
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { HttpClientModule } from '@angular/common/http';
import { MatMenuModule } from '@angular/material/menu';


const routes: Routes = [
  {
    path: '',
    component: QuestionsOverviewComponent
  }
];

@NgModule({
  declarations: [
    QuestionsOverviewComponent  
  ],
  imports: [
    CommonModule,  
    RouterModule.forChild(routes) ,
    MatTableModule,  
    MatPaginatorModule,  
    MatSortModule, 
    HttpClientModule, 
    MatMenuModule,
   
  ]
})
export class QuestionsModule { }
