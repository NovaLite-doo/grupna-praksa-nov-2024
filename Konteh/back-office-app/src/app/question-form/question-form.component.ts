import { Component, Input } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-question-form',
  templateUrl: './question-form.component.html',
  styleUrl: './question-form.component.css'
})
export class QuestionFormComponent {
  @Input() questionForm: FormGroup = new FormGroup([]);

  get answers(): FormArray {
    return this.questionForm.get('answers') as FormArray;
  }
}
