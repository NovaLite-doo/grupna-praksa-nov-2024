import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-submit-dialog',
  templateUrl: './submit-dialog.component.html',
  styleUrl: './submit-dialog.component.css'
})
export class SubmitDialogComponent {
  timerExpired: boolean;

  constructor(
    public dialogRef: MatDialogRef<SubmitDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { timerExpired: boolean }
  ) {
    this.timerExpired = data.timerExpired;
  }

  onConfirm(): void {
    this.dialogRef.close(true); 
  }

  onCancel(): void {
    if (!this.timerExpired) {
      this.dialogRef.close(false);  
    }
  }
}
