import { Component, Input } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { QuestionCategory, QuestionType } from '../../../../api/api-reference';
import { QuestionForm } from '../models/question-form.model';
import { FORM_FIELD_ERROR_KEY } from '../../../../shared/validation/validation';

@Component({
  selector: 'app-question-form',
  templateUrl: './question-form.component.html',
  styleUrl: './question-form.component.css'
})
export class QuestionFormComponent {
  questionCategories = QuestionCategory;
  questionTypes = QuestionType;

  @Input() questionForm = new QuestionForm();

  get answers(): FormArray<FormGroup> {
    return this.questionForm.get('answers') as FormArray<FormGroup>;
  }

  get typeKeys() {
    return Object.keys(this.questionTypes)
      .filter(key => isNaN(Number(key))); 
  }

  get categoryKeys() {
    return Object.keys(this.questionCategories)
      .filter(key => isNaN(Number(key))); 
  }

  getCategoryValue(category: string): number {
    return this.questionCategories[category as keyof typeof this.questionCategories];
  }

  getTypeValue(type: string): number {
    return this.questionTypes[type as keyof typeof this.questionTypes];
  }

  addAnswer() {
    const answerFormGroup = new FormGroup({
      id: new FormControl(),
      text: new FormControl('', [Validators.required]),
      isCorrect: new FormControl(false)
    });
    this.answers.push(answerFormGroup);
  }

  removeAnswer(index: number) {
    this.answers.removeAt(index);
  }

  hasErrors = (formControlName: string) => {
    const errors = this.questionForm?.get(formControlName)?.errors;
    return errors && errors[FORM_FIELD_ERROR_KEY];
  }

  getErrors = (formControlName: string) => {
    const errors = this.questionForm?.get(formControlName)?.errors;
    if (!errors) {
      return null;
    }
    return errors[FORM_FIELD_ERROR_KEY];
  }
}
