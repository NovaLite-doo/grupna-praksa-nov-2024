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
  candidateForm!: FormGroup;
  YearOfStudy = YearOfStudy;

  constructor(
    private router: Router, 
    private examClient: ExamClient 
  ) { }

  ngOnInit(): void {
    this.candidateForm = new FormGroup({
      'name': new FormControl('', [Validators.required, Validators.minLength(2)]),
      'surname': new FormControl('', [Validators.required, Validators.minLength(2)]),
      'email': new FormControl('', [Validators.required, Validators.email]),
      'faculty': new FormControl('', [Validators.required]),
      'major': new FormControl('', [Validators.required]),
      'yearOfStudy': new FormControl('', [Validators.required]) 
    });
  }

  onSubmit(): void {
    if (this.candidateForm.valid) {
      const candidateData = this.candidateForm.value;
      const examCommand = new CreateExamCommand();
      examCommand.email = candidateData.email;
      examCommand.faculty = candidateData.faculty;
      examCommand.major = candidateData.major;
      examCommand.name = candidateData.name;
      examCommand.surname = candidateData.surname;
      examCommand.yearOfStudy = candidateData.yearOfStudy as YearOfStudy;

      this.examClient.generateExam(examCommand).subscribe(
        (response) => {
          console.log('Exam generated successfully:', response);
          const examId = 3;
          this.router.navigate(['/exam-overview', examId]); 
        },
        (error) => {
          console.error('Error generating exam:', error);
        }
      );
    } else {
      console.log('Form not valid!');
    }
  }

}