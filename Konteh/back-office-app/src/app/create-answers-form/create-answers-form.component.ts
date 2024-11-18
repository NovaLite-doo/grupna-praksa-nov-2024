import { Component, Input } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { DeleteDialog } from '../delete-dialog/delete-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-create-answers-form',
  templateUrl: './create-answers-form.component.html',
  styleUrl: './create-answers-form.component.css'
})
export class CreateAnswersFormComponent {
  @Input() answers: FormArray<FormGroup> = new FormArray<FormGroup>([]);

  constructor(public dialog: MatDialog) {}

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

  confirmRemoval(index: number) {
    this.dialog
      .open(DeleteDialog, {
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
