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
      'yearOfStudy': new FormControl(null, [Validators.required])
    });
  }

  onSubmit(): void {
    if (this.candidateForm.valid) {
      const candidateData = this.candidateForm.value;
      console.log('Form data:', candidateData); 
      const examCommand = new CreateExamCommand();
      examCommand.email = candidateData.email;
      examCommand.faculty = candidateData.faculty;
      examCommand.major = candidateData.major;
      examCommand.name = candidateData.name;
      examCommand.surname = candidateData.surname;
      examCommand.yearOfStudy = Number(candidateData.yearOfStudy); 

      this.examClient.generateExam(examCommand).subscribe(
        (response) => {
          console.log('Exam generated successfully:', response);

          const reader = new FileReader();
          reader.onload = () => {
            try {
              const jsonResponse = JSON.parse(reader.result as string);
              const examId = jsonResponse.examId;
              if (examId) {
                console.log('Generated Exam ID:', examId);
                this.router.navigate(['/exam-overview', examId]);
              } else {
                console.error('examId not found in the response');
              }
            } catch (error) {
              console.error('Error parsing Blob data:', error);
            }
          };
  
          reader.readAsText(response.data);
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
