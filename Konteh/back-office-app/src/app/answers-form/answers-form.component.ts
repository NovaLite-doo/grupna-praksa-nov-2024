import { Component, Input } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-answers-form',
  templateUrl: './answers-form.component.html',
  styleUrl: './answers-form.component.css'
})
export class AnswersFormComponent {
  @Input() answersForm: FormArray<FormGroup> = new FormArray<FormGroup>([]);

  constructor(public dialog: MatDialog) {}

  get parent(): FormGroup {
    return this.answersForm.parent as FormGroup
  }

  addAnswer() {
    const answerFormGroup = new FormGroup({
      id: new FormControl(-1),
      text: new FormControl('', [Validators.required]),
      isCorrect: new FormControl(false)
    });
    this.answersForm?.push(answerFormGroup);
  }

  removeAnswer(index: number) {
    this.answersForm?.removeAt(index);
  }

  confirmRemoval(index: number) {
    this.dialog
      .open(ConfirmationDialogComponent, {
        data: {
          title: "Answer deletion",
          message: "Confirm answer deletion?"
        }
      })
      .afterClosed()
      .subscribe((confirmation: Boolean) => {
        if(confirmation) {
          this.removeAnswer(index);
        }
      });
  }
}
