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
import { DeleteConfirmDialogComponent } from './delete-confirm-dialog/delete-confirm-dialog.component';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';



const routes: Routes = [
  {
    path: '',
    component: QuestionsOverviewComponent
  }
];

@NgModule({
  declarations: [
    QuestionsOverviewComponent,
    DeleteConfirmDialogComponent  
  ],
  imports: [
    CommonModule,  
    RouterModule.forChild(routes) ,
    MatTableModule,  
    MatPaginatorModule,  
    MatSortModule, 
    HttpClientModule, 
    MatMenuModule,
    MatIconModule,
    MatDialogModule,
    MatSelectModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule
   
  ]
})
export class QuestionsModule { }