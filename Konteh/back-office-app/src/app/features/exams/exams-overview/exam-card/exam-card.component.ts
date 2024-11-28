import { Component, Input } from '@angular/core';
import { SearchExamsExamResponse } from '../../../../api/api-reference';

@Component({
  selector: 'app-exam-card',
  templateUrl: './exam-card.component.html',
  styleUrl: './exam-card.component.css'
})
export class ExamCardComponent {
  @Input() exam!: SearchExamsExamResponse

  get yearOfStudy(): string | number {
    if(this.exam.candidate?.yearOfStudy == 4) {
      return 'Master';
    }
    return Number(this.exam.candidate?.yearOfStudy) + 1;
  }
}
