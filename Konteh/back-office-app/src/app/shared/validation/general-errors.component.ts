import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { ReactiveFormsModule, UntypedFormGroup } from "@angular/forms";
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
    selector: 'app-general-errors',
    template: `
    <div *ngIf="form.errors">
      <mat-error *ngFor="let error of generalErrors">
        <span class="mat-body-2">{{error}}</span>
      </mat-error>
    </div>
    `,
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule, MatFormFieldModule]
})
export class GeneralErrorsComponent {
    @Input() form!: UntypedFormGroup;

    get generalErrors() {
        return this.form.errors!["general"];
    }
}