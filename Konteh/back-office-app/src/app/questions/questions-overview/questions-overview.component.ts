import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatMenuTrigger } from '@angular/material/menu';
import { Question, QuestionsService } from '../../services/questions.service';

@Component({
  selector: 'app-questions-overview',
  templateUrl: './questions-overview.component.html',
  styleUrl: './questions-overview.component.css'
})
export class QuestionsOverviewComponent {

  displayedColumns: string[] = ['id', 'text', 'answers'];
  dataSource = new MatTableDataSource<Question>([]);
  selectedQuestion: Question | null = null; 

  @ViewChild(MatPaginator) paginator!: MatPaginator;
 

  constructor(private questionsService: QuestionsService) {}

  ngOnInit(): void {
  
    this.questionsService.getQuestions().subscribe(
      (data) => {
       
        this.dataSource.data = data;
        
      
        if (this.paginator) {
          this.dataSource.paginator = this.paginator;
        }
       
      },
      (error) => {
        console.error('Greška prilikom učitavanja pitanja: ', error);
      }
    );
  }

  onRowClick(question: Question): void {
   
    this.selectedQuestion = question;
  }

  onEdit(question: Question): void {
    
   
    
  }

  onDelete(question: Question): void {
    if (confirm(`Are you sure you want to delete this question: "${question.text}"?`)) {
      this.questionsService.deleteQuestion(question.id).subscribe(
        () => {
      
          this.dataSource.data = this.dataSource.data.filter(q => q.id !== question.id);
        },
        (error: HttpErrorResponse) => {  
          console.error('Error while deleting the question: ', error);
          alert('An error occurred while deleting the question.');
        }
      );
    }
  }




}
