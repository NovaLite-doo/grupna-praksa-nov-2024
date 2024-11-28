import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-answer-selection',
  templateUrl: './answer-selection.component.html',
  styleUrl: './answer-selection.component.css'
})
export class AnswerSelectionComponent {
  @Input() questionId!: number;
  @Input() answers: any[] = [];
  @Input() selectedAnswers: number[] = [];
  @Input() questionType!: number; 

  @Output() answerSelected = new EventEmitter<{ questionId: number, answerId: number, isRadio: boolean }>();

  onAnswerSelected(answerId: number, isRadio: boolean): void {
    this.answerSelected.emit({ questionId: this.questionId, answerId, isRadio });
  }

  isAnswerSelected(answerId: number): boolean {
    return this.selectedAnswers.includes(answerId);
  }
}
