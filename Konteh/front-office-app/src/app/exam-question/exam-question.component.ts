import { Component, Input } from '@angular/core';
import { QuestionType } from '../api/api-reference';
import { ExamQuestionForm } from '../exam-overview/models/exam-question-form.model';
import { FormArray } from '@angular/forms';
import { AnswerForm } from '../exam-overview/models/answer-form.model';

@Component({
  selector: 'app-exam-question',
  templateUrl: './exam-question.component.html',
  styleUrl: './exam-question.component.css'
})
export class ExamQuestionComponent {
  @Input() examQuestion!: ExamQuestionForm;
  type = QuestionType;

  constructor() { }

  get questionType() {
    return this.examQuestion.controls['type'].value as QuestionType | null;
  }

  get answers() {
    return this.examQuestion.controls['answers'] as FormArray<AnswerForm>;
  }

  isAnswerSelected = (answerId: number) => {
    return answerId === this.examQuestion.selectedAnswer;
  }

  checkBoxAnswerSelected = (answer: AnswerForm) => {
    answer.isSelected.patchValue(!answer.isSelected.value);
  }
}
