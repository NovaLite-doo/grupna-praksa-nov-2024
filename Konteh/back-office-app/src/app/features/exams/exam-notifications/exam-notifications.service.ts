import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, BehaviorSubject } from 'rxjs';
import { ExamNotification } from './models/exam-notification';

@Injectable({
  providedIn: 'root'
})
export class ExamNotificationsService {
  private hubConnection: signalR.HubConnection;
  private notificationSubject = new BehaviorSubject<ExamNotification | null>(null);

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

  receiveNotification(): Observable<ExamNotification | null> {
    this.hubConnection.on('ReceiveNotification', (message: ExamNotification) => {
      this.notificationSubject.next(message);
    });
    return this.notificationSubject.asObservable();
  }
}
