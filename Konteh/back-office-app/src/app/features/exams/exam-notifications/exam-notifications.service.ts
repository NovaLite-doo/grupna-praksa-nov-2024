import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ExamNotificationsService {
  private hubConnection: signalR.HubConnection;
  private notificationSubject = new BehaviorSubject<any>(null); // any for test purposes, a DTO will be created later

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

  receiveNotification(): Observable<any> {
    this.hubConnection.on('ReceiveNotification', (message: any) => {
      this.notificationSubject.next(message);
    });
    return this.notificationSubject.asObservable();
  }
}
