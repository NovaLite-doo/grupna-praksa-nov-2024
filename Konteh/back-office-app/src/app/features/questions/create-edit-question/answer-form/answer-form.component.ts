import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { FormArray, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-answer-form',
  templateUrl: './answer-form.component.html',
  styleUrl: './answer-form.component.css'
})
export class AnswerFormComponent {
  @Input() answerForm: FormGroup = new FormGroup([]);
  @Input() index: number = -1;

  constructor(private dialog: MatDialog) {}

  get rootGroup(): FormGroup {
    let parent = this.answerForm.parent as FormGroup;
    parent = parent.parent as FormGroup;

    return parent;
  }

  get parent(): FormArray {
    return this.answerForm.parent as FormArray
  }
  
  @Output() removeAnswerEvent = new EventEmitter<number>();
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
        if(confirmation) {
          this.removeAnswer();
        }
      });
  }
}
