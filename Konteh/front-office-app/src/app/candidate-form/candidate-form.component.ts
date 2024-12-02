import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CreateExamCommand, ExamClient, YearOfStudy } from '../api/api-reference';
import { FORM_FIELD_ERROR_KEY, setServerSideValidationError } from '../shared/validation/validation';

@Component({
  selector: 'app-candidate-form',
  templateUrl: './candidate-form.component.html',
  styleUrl: './candidate-form.component.css'
})
export class CandidateFormComponent {
  emailRegex: RegExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  candidateForm: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
    surname: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.pattern(this.emailRegex)]),
    faculty: new FormControl('', [Validators.required]),
    major: new FormControl('', [Validators.required]),
    yearOfStudy: new FormControl(null, [Validators.required])
  });

  YearOfStudy = YearOfStudy;

  constructor(
    private router: Router,
    private examClient: ExamClient
  ) { }

  onSubmit(): void {
    if (!this.candidateForm.valid) {
      return;
    }
    const candidateData = this.candidateForm.value;

    const examCommand = new CreateExamCommand({
      email: candidateData.email,
      faculty: candidateData.faculty,
      major: candidateData.major,
      name: candidateData.name,
      surname: candidateData.surname,
      yearOfStudy: Number(candidateData.yearOfStudy),
    });

    this.examClient.generateExam(examCommand).subscribe({
      next: response => {
        const examId = response;
        if (examId) {
          this.router.navigate(['/exam-overview', examId]);
        }
      },
      error: errors => setServerSideValidationError(errors, this.candidateForm)
    });
  }

  getYearOfStudyLabels(): string[] {
    return Object.keys(YearOfStudy)
      .filter(key => isNaN(Number(key)))
      .map(key => key.replace(/([A-Z])/g, ' $1').trim());
  }

  hasErrors = (formControlName: string) => {
    const errors = this.candidateForm?.get(formControlName)?.errors;
    return errors && errors[FORM_FIELD_ERROR_KEY];
  }

  getErrors = (formControlName: string) => {
    const errors = this.candidateForm?.get(formControlName)?.errors;
    if (!errors) {
      return null;
    }
    return errors[FORM_FIELD_ERROR_KEY];
  }
}