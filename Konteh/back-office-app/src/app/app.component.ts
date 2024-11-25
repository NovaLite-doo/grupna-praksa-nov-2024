import { Component } from '@angular/core';
import { ExamNotificationsService } from './features/exams/exam-notifications/exam-notifications.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'back-office-app';

  constructor(private examNotifier: ExamNotificationsService) {}

  ngOnInit(): void {
    this.examNotifier.startConnection().subscribe(() => {
      this.examNotifier.receiveMessage().subscribe((message) => {
        console.log(message);
      });
    });
  }
}
