import { Component } from '@angular/core';
import { ExamsClient, SearchExamsExamResponse } from '../../../api/api-reference';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-exams-overview',
  templateUrl: './exams-overview.component.html',
  styleUrl: './exams-overview.component.css'
})
export class ExamsOverviewComponent {
  status: string = "All";
  exams: SearchExamsExamResponse[] = [];

  search: string | null = null;
  searchFormControl: FormControl = new FormControl('');

  constructor(private examsClient: ExamsClient) {}

  ngOnInit(): void {
    this.loadExams();

    this.searchExams();
  }

  loadExams(): void {
    this.examsClient.search(this.search).subscribe(
      exams => {
        this.exams = exams;
      }
    );
  }

  searchExams(): void {
    this.searchFormControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(searchText => {
        this.search = searchText ? searchText : null;
        this.loadExams();
      })
  }
}
