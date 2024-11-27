import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-exam-card',
  templateUrl: './exam-card.component.html',
  styleUrl: './exam-card.component.css'
})
export class ExamCardComponent {
  @Input() isCompleted: boolean = false
}
