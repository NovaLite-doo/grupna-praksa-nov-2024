import { Component } from '@angular/core';
import { ExamsClient, SearchExamsCandidateResponse, SearchExamsExamResponse } from '../../../api/api-reference';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { ExamNotificationsService } from '../exam-notifications/exam-notifications.service';
import { ExamNotification } from '../exam-notifications/models/exam-notification';

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
  ) {}

  ngOnInit(): void {
    this.loadExams();

    this.searchExams();

    this.receiveNotifications();
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

  receiveNotifications() {
    this.examNotificationsService.receiveNotification().subscribe(
      (notification: ExamNotification | null) => {
        console.log(notification);
        if(notification) {
          if(notification.isCompleted) {
            this.updateExam(notification);
          } else {
            this.addExam(notification!);
          }
        }
      }
    );
  }

  updateExam(notification: ExamNotification) {
    const exam = this.exams.find(x => x.id === notification.id);

    if (exam) {
      exam.isCompleted = true;
      exam.questionCount = notification.questionCount;
      exam.correctAnswerCount = notification.correctAnswerCount;
      exam.score = notification.score;
    }
  }

  addExam(notification: ExamNotification) {
    const newCandidate = new SearchExamsCandidateResponse();
    newCandidate.name = notification.candidate?.name;
    newCandidate.surname = notification.candidate?.surname;
    newCandidate.email = notification.candidate?.email;
    newCandidate.faculty = notification.candidate?.faculty;
    newCandidate.major = notification.candidate?.major;
    newCandidate.yearOfStudy = notification.candidate?.yearOfStudy;

    const newExam = new SearchExamsExamResponse();
    newExam.id = notification.id;
    newExam.isCompleted = false;
    newExam.candidate = newCandidate;

    this.exams.unshift(newExam);
  }
}
