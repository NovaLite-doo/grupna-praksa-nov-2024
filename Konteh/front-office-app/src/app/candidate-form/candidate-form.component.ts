import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CreateExamCommand, ExamClient, YearOfStudy } from '../api/api-reference';

@Component({
  selector: 'app-candidate-form',
  templateUrl: './candidate-form.component.html',
  styleUrl: './candidate-form.component.css'
})
export class CandidateFormComponent {
  emailRegex: RegExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  candidateForm: FormGroup = new FormGroup({
    'name': new FormControl('', [Validators.required, Validators.minLength(2)]),
    'surname': new FormControl('', [Validators.required, Validators.minLength(2)]),
    'email': new FormControl('', [Validators.required,Validators.pattern(this.emailRegex)]),
    'faculty': new FormControl('', [Validators.required]),
    'major': new FormControl('', [Validators.required]),
    'yearOfStudy': new FormControl(null, [Validators.required])
  });

  YearOfStudy = YearOfStudy;

  constructor(
    private router: Router, 
    private examClient: ExamClient 
  ) { }

  onSubmit(): void {
    if (this.candidateForm.valid) {
      const candidateData = this.candidateForm.value;
      
      const examCommand = new CreateExamCommand({
        email: candidateData.email,
        faculty: candidateData.faculty,
        major: candidateData.major,
        name: candidateData.name,
        surname: candidateData.surname,
        yearOfStudy: Number(candidateData.yearOfStudy),  
      });
  
      this.examClient.generateExam(examCommand).subscribe(response => {
        const examId = response;
        if (examId) {
          this.router.navigate(['/exam-overview', examId]);
        }
      });
    }
  }

  getYearOfStudyLabels(): string[] {
    return Object.keys(YearOfStudy)
      .filter(key => isNaN(Number(key)))
      .map(key => key.replace(/([A-Z])/g, ' $1').trim());
  }
}