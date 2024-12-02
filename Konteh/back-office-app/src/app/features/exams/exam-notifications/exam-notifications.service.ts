import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, BehaviorSubject } from 'rxjs';
import { SearchExamsExamResponse } from '../../../api/api-reference';

@Injectable({
  providedIn: 'root'
})
export class ExamNotificationsService {
  private hubConnection: signalR.HubConnection;
  private notificationSubject = new BehaviorSubject<SearchExamsExamResponse | null>(null);

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7285/exam-notifications')
      .build();
  }

  startConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          observer.error(error);
          setTimeout(this.startConnection, 5000);
        });
    });
  }

  receiveNotification(): Observable<SearchExamsExamResponse | null> {
    this.hubConnection.on('ReceiveNotification', (message: SearchExamsExamResponse) => {
      this.notificationSubject.next(message);
    });
    return this.notificationSubject.asObservable();
  }
}
