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
  public notificationSubject$ = this.notificationSubject.asObservable();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7285/exam-notifications')
      .build();
  }

  async startConnection() {
    await this.hubConnection.start();
    
    this.hubConnection.on('ReceiveNotification', (message: SearchExamsExamResponse) => {
      this.notificationSubject.next(message);
    });
  }
}
