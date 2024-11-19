import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrl: './confirmation-dialog.component.css'
})
export class ConfirmationDialogComponent {
  constructor(
    public dialog: MatDialogRef<ConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

    close(): void {
      this.dialog.close(false);
    }

    confirm(): void {
      this.dialog.close(true);
    }
}