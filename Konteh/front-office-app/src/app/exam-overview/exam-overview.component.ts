import { Component } from '@angular/core';
import { ExamClient, IGetExamByIdResponse, SubmitExamCommand, SubmitExamExamQuestionDto } from '../api/api-reference';
import { ActivatedRoute, Router } from '@angular/router';
import { retryWhen } from 'rxjs';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { ExamForm } from './models/exam-form.model';
import { ExamQuestionForm } from './models/exam-question-form.model';
import { SubmitDialogComponent } from '../submit-dialog/submit-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-exam-overview',
  templateUrl: './exam-overview.component.html',
  styleUrl: './exam-overview.component.css'
})
export class ExamOverviewComponent {
  exam: IGetExamByIdResponse | null = null;
  currentQuestionIndex: number = 0;
  examFormGroup = new ExamForm();
  totalTime: number = 200;
  isExpired: boolean = false;

  constructor(private examClient: ExamClient, private route: ActivatedRoute, private router: Router, private dialog: MatDialog ) { }

  ngOnInit(): void {
    const examId = Number(this.route.snapshot.paramMap.get('id'));
    if (examId) {
      this.fetchExam(examId);
    }
  }

  fetchExam(id: number): void {
    this.examClient.getExamById(id).subscribe(
      (response: IGetExamByIdResponse) => {
        this.exam = response;
        this.examFormGroup.patchValue({
          id: response.id
        });
        response.questions?.forEach(question => (this.examFormGroup.controls['questions'] as FormArray<ExamQuestionForm>).push(new ExamQuestionForm(question)));
      },
    );
  }

  submitExam(): void {
    const submitCommand = new SubmitExamCommand({
      examId: this.exam!.id,
      examQuestions: this.examFormGroup.questions.value.map(x => new SubmitExamExamQuestionDto({
        id: x.id,
        submittedAnswers: x.selectedAnswer !== undefined && x.selectedAnswer !== null ? [x.selectedAnswer] : x.answers.filter((a: any) => a.isSelected).map((a: any) => a.id)
      }))
    });

    this.examClient.submitExam(submitCommand).subscribe(
      () => {
        this.router.navigate(['/thank-you']);
      },
      (error) => {
        console.error('Error submitting exam:', error);
      }
    );
  }

  nextQuestion(): void {
    if (this.currentQuestionIndex < this.totalQuestions - 1) {
      this.currentQuestionIndex++;
    }
  }

  previousQuestion(): void {
    if (this.currentQuestionIndex > 0) {
      this.currentQuestionIndex--;
    }
  }

  isLastQuestion(): boolean {
    return this.currentQuestionIndex === this.totalQuestions - 1;
  }

  get currentQuestion() {
    return this.examFormGroup.questions.controls[this.currentQuestionIndex];
  }

  get totalQuestions(): number {
    return this.exam?.questions?.length ?? 0;
  }

  onTimerExpired(): void {
    this.isExpired = true; 
    this.submitExam();
    this.openSubmitDialog();
  }

  openSubmitDialog(): void {
    const dialogRef = this.dialog.open(SubmitDialogComponent, {
      width: '300px',
      data: {
        timerExpired: this.isExpired
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!this.isExpired && result) {
        this.submitExam();
      }
    });
  }
}