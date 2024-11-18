import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, UntypedFormGroup, ValidationErrors, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-answers-form',
  templateUrl: './create-answers-form.component.html',
  styleUrl: './create-answers-form.component.css'
})
export class CreateAnswersFormComponent {
  @Input() answers: FormArray<FormGroup> = new FormArray<FormGroup>([]);

  constructor() {}

  get parent(): FormGroup {
    return this.answers.parent as FormGroup
  }

  addAnswer() {
    const answerFormGroup = new FormGroup({
      text: new FormControl('', [Validators.required]),
      isCorrect: new FormControl(false)
    });
    this.answers?.push(answerFormGroup);
  }

  removeAnswer(index: number) {
    this.answers?.removeAt(index);
  }
}
