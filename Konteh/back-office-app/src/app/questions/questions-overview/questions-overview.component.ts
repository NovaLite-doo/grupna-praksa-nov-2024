import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatMenuTrigger } from '@angular/material/menu';
import { Question, QuestionsService } from '../../services/questions.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteConfirmDialogComponent } from '../delete-confirm-dialog/delete-confirm-dialog.component';
import { QuestionsClient } from '../../api/api-reference';

@Component({
  selector: 'app-questions-overview',
  templateUrl: './questions-overview.component.html',
  styleUrl: './questions-overview.component.css'
})
export class QuestionsOverviewComponent {

  displayedColumns: string[] = [ 'text', 'actions'];
  dataSource = new MatTableDataSource<Question>([]);
  selectedQuestion: Question | null = null; 

  @ViewChild(MatPaginator) paginator!: MatPaginator;
 

  constructor(private questionsClient: QuestionsClient,   private dialog: MatDialog ) {}

  ngOnInit(): void {
    this.questionsClient.getAll().subscribe(
      (data) => {
        // Assert the type of data to 'Question[]'
        this.dataSource.data = data as Question[];
        
        if (this.paginator) {
          this.dataSource.paginator = this.paginator;
        }
      },
      (error) => {
        console.error('Error loading questions: ', error);
      }
    );
  }
  

  onRowClick(question: Question): void {
   
    this.selectedQuestion = question;
  }

  onEdit(question: Question): void {
    
   
    
  }

  onDelete(question: Question): void {
    const dialogRef = this.dialog.open(DeleteConfirmDialogComponent, {
      data: { questionText: question.text }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        
        this.deleteQuestion(question);
      }
    });
  }

  deleteQuestion(question: Question): void {
    this.questionsClient.delete(question.id).subscribe(
      () => {
        // Remove from the table
        this.dataSource.data = this.dataSource.data.filter(q => q.id !== question.id);
      },
      (error) => {
        console.error('Error while deleting the question: ', error);
        alert('An error occurred while deleting the question.');
      }
    );
  }




}
