import { Component, Input } from '@angular/core';
import { SearchExamsExamResponse } from '../../../../api/api-reference';
import { ExamStatus } from '../../../../api/api-reference';

@Component({
  selector: 'app-exam-card',
  templateUrl: './exam-card.component.html',
  styleUrl: './exam-card.component.css'
})
export class ExamCardComponent {
  @Input() exam!: SearchExamsExamResponse

  get isCompleted(): boolean {
    return this.exam.status == ExamStatus.Completed;
  }
}
