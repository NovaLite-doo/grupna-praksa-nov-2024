import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Question } from '../../services/questions.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteConfirmDialogComponent } from '../delete-confirm-dialog/delete-confirm-dialog.component';
import {  GetCategoriesCategoryResponse,  QuestionsClient } from '../../api/api-reference';
import { debounceTime, distinctUntilChanged, Subject, Subscription, switchMap } from 'rxjs';



@Component({
  selector: 'app-questions-overview',
  templateUrl: './questions-overview.component.html',
  styleUrl: './questions-overview.component.css'
})
export class QuestionsOverviewComponent {

  displayedColumns: string[] = [ 'text', 'actions'];
  dataSource = new MatTableDataSource<Question>([]);
  totalCount = 0; 
  pageSize = 10;  
  pageNumber = 1;
  selectedQuestion: Question | null = null; 
  searchText: string = ''; 
  selectedCategory: GetCategoriesCategoryResponse | null = null; 
  categories: GetCategoriesCategoryResponse[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  private searchSubject: Subject<string> = new Subject<string>();
  private searchSubscription: Subscription | null = null;

  
  constructor(private questionsClient: QuestionsClient,   private dialog: MatDialog ) {}

  
  ngOnInit(): void {
  this.loadCategories();
  this.loadQuestions(this.pageNumber, this.pageSize);

  this.searchSubscription = this.searchSubject.pipe(
    debounceTime(500),
    distinctUntilChanged(),
    switchMap((searchText) => {
      const categoryId = this.selectedCategory ? this.selectedCategory.id : undefined;
      if (!searchText.trim()) {
        return this.questionsClient.getAll(this.pageNumber, this.pageSize);  
      } else {
        return this.questionsClient.search(searchText, this.pageNumber, this.pageSize, categoryId);
      }
    })
  ).subscribe(
    (response) => {


      const questions = (response.questions ?? [])
        .filter(q => q.id !== undefined)
        .map(q => ({
          id: q.id ?? 0,
          text: q.text ?? '',
          answers: []
        }));

     
      if ('totalCount' in response) {
        
        this.totalCount = response.totalCount ?? 0;
      } else if ('pageCount' in response) {
        
        this.totalCount = response.pageCount ?? 0;
      } else {
        
        this.totalCount = 0;
      }

      this.dataSource.data = questions;

      if (this.paginator) {
        this.paginator.length = this.totalCount;
      }
    },
    
  );
}

  
  

  ngOnDestroy(): void {
   
    if (this.searchSubscription) {
      this.searchSubscription.unsubscribe();
    }
  }
  
  loadQuestions(pageNumber: number, pageSize: number): void {
    this.questionsClient.getAll(pageNumber, pageSize).subscribe(
      (response) => {
       
        const questions = (response.questions ?? [])
          .filter(q => q.id !== undefined) 
          .map(q => ({
            id: q.id ?? 0,
            text: q.text ?? '', 
            answers: (q.answers ?? []).map(a => ({
              id: a.id ?? 0, 
              text: a.text ?? '' 
            })) 
          }));

        
        this.dataSource.data = questions;
        this.totalCount = response.totalCount ?? 0;

       
        if (this.paginator) {
          this.paginator.length = this.totalCount;
        }
      },
      
    );
  }

  loadCategories(): void {
    this.questionsClient.getCategories().subscribe(
      (response) => {
       
        this.categories = response.categories ?? []; 
      },
      
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
       
        this.dataSource.data = this.dataSource.data.filter(q => q.id !== question.id);
      },
      
    );
  }

  searchQuestions(pageNumber: number, pageSize: number): void {
    
    let search = this.searchText !== '' ? this.searchText : undefined;
    const categoryId = this.selectedCategory ? this.selectedCategory.id : undefined;
  
   
    this.questionsClient.search(search, pageNumber, pageSize, categoryId).subscribe(
      (response) => {
        const questions = (response.questions ?? [])
          .filter(q => q.id !== undefined)
          .map(q => ({
            id: q.id ?? 0,
            text: q.text ?? '',
            answers: [] 
          }));
  
        this.dataSource.data = questions;
        this.totalCount = response.pageCount ?? 0;
  
        if (this.paginator) {
          this.paginator.length = this.totalCount;
        }
      },
      
    );
  }
  
  
  onSearchChange(): void {
   
    if (this.searchText.trim() === '') {
      this.searchSubject.next('');  
      this.loadQuestions(this.pageNumber, this.pageSize);
    } else {
      this.searchSubject.next(this.searchText);  
    }
  }

  onPageChange(event: any): void {
    this.pageNumber = event.pageIndex + 1;  
    this.pageSize = event.pageSize;  

    if (this.searchText || this.selectedCategory) {
      this.searchQuestions(this.pageNumber, this.pageSize);  
    } else {
      this.loadQuestions(this.pageNumber, this.pageSize); 
    }
  }

  onCategoryChange(): void {
    this.pageNumber = 1; 
    this.searchQuestions(this.pageNumber, this.pageSize); 
  }


}