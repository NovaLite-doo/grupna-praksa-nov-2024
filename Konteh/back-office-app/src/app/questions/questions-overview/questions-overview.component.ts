import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { DeleteConfirmDialogComponent } from '../delete-confirm-dialog/delete-confirm-dialog.component';
import { debounceTime, distinctUntilChanged, filter } from 'rxjs';
import { QuestionCategory, QuestionsClient, SearchQuestionsResponse } from '../../api/api-reference';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-questions-overview',
  templateUrl: './questions-overview.component.html',
  styleUrl: './questions-overview.component.css'
})
export class QuestionsOverviewComponent {

  displayedColumns: string[] = ['text', 'category', 'actions'];
  dataSource = new MatTableDataSource<SearchQuestionsResponse>([]);
  pageNumber: number = 0;
  pageSize: number = 10;
  pageSizeOptions = [5, 10, 50];
  totalItems = 0;
  searchText = '';
  selectedCategory: QuestionCategory | null = null;
  categoryEnumKeys = Object.keys(QuestionCategory).filter(key => isNaN(Number(key))) .map(key => key);
  searchTextControl: FormControl = new FormControl('');

  constructor(private questionsClient: QuestionsClient, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.searchTextControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(searchText => {
        this.searchText = searchText;
        this.loadQuestions();
      })

    this.loadQuestions();
  }

  loadQuestions(): void {
    this.questionsClient.search(this.searchText, this.pageNumber, this.pageSize, this.selectedCategory)
      .subscribe(response => {
        this.totalItems = response.totalItems!;
        this.dataSource.data = response.questions ?? [];
      });
  }

  onEdit(question: SearchQuestionsResponse): void { }

  onDelete(question: SearchQuestionsResponse): void {
    const dialogRef = this.dialog.open(DeleteConfirmDialogComponent, {
      data: { questionText: question.text }
    });

    dialogRef.afterClosed()
      .pipe(filter(result => result))
      .subscribe(_ => this.deleteQuestion(question));
  }

  deleteQuestion(question: SearchQuestionsResponse): void {
    this.questionsClient.delete(question.id!).subscribe(
      _ => this.dataSource.data = this.dataSource.data.filter(q => q.id !== question.id)
    );
  }

  onPageChange(event: any): void {
    this.pageNumber = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadQuestions();
  }

  onCategoryChange(): void {
    this.pageNumber = 0;
    this.loadQuestions();
  }
}
