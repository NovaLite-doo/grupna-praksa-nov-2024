import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { FormArray, FormControl, FormGroup, UntypedFormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AnswerForm } from '../models/answer-form.model';

@Component({
  selector: 'app-answer-form',
  templateUrl: './answer-form.component.html',
  styleUrl: './answer-form.component.css'
})
export class AnswerFormComponent {
  @Input() answerForm?: AnswerForm;
  @Input() index: number = -1;
  @Output() removeAnswerEvent = new EventEmitter<number>();

  constructor(private dialog: MatDialog) { }

  get rootGroup(): FormGroup {
    let parent = this.answerForm?.parent as FormGroup;
    parent = parent.parent as FormGroup;

    return parent;
  }

  get parent(): FormArray {
    return this.answerForm?.parent as FormArray
  }

  removeAnswer() {
    this.removeAnswerEvent.emit(this.index);
  }

  confirmRemoval() {
    this.dialog
      .open(ConfirmationDialogComponent, {
        data: {
          title: "Answer deletion",
          message: "Confirm answer deletion?"
        }
      })
      .afterClosed()
      .subscribe((confirmation: Boolean) => {
        if (confirmation) {
          this.removeAnswer();
        }
      });
  }
}
