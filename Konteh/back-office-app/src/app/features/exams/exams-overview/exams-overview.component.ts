import { Component } from '@angular/core';
import { ExamsClient, ExamStatus, SearchExamsExamResponse } from '../../../api/api-reference';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { ExamNotificationsService } from '../exam-notifications/exam-notifications.service';

@Component({
  selector: 'app-exams-overview',
  templateUrl: './exams-overview.component.html',
  styleUrl: './exams-overview.component.css'
})
export class ExamsOverviewComponent {
  exams: SearchExamsExamResponse[] = [];

  search: string | null = null;
  searchFormControl: FormControl = new FormControl('');

  constructor(
    private examsClient: ExamsClient,
    private examNotificationsService: ExamNotificationsService
  ) { }

  ngOnInit(): void {
    this.loadExams();

    this.searchFormControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(searchText => {
        this.search = searchText ? searchText : null;
        this.loadExams();
      })

    this.receiveNotifications();
  }

  loadExams(): void {
    this.examsClient.search(this.search).subscribe(
      exams => {
        this.exams = exams;
      }
    );
  }

  receiveNotifications() {
    this.examNotificationsService.notificationSubject$.subscribe(
      (notification: SearchExamsExamResponse | null) => {
        if (notification) {
          if (notification.status == ExamStatus.Completed) {
            this.updateExam(notification);
          } else {
            this.addExam(notification!);
          }
        }
      }
    );
  }

  updateExam(notification: SearchExamsExamResponse) {
    const exam = this.exams.find(x => x.id === notification.id);

    if (exam) {
      exam.status = notification.status;
      exam.score = notification.score;
    }
  }

  addExam(notification: SearchExamsExamResponse) {
    this.exams.unshift(notification);
  }
}
