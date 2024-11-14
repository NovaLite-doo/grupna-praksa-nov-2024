import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


export interface Answer {
  id: number;
  text: string;
}

export interface Question {
  id: number;
  text: string;
  answers: Answer[];
}

@Injectable({
  providedIn: 'root'
})
export class QuestionsService {

  private apiUrl = 'https://localhost:7285/questions'; 

  constructor(private http: HttpClient) { }

  getQuestions(): Observable<Question[]> {
    return this.http.get<Question[]>(this.apiUrl);
  }

  deleteQuestion(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
