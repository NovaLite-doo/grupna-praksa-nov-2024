import { Component } from '@angular/core';
import { ExamClient, IGetExamByIdResponse, SubmitExamCommand, SubmitExamExamQuestionDto } from '../api/api-reference';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-exam-overview',
  templateUrl: './exam-overview.component.html',
  styleUrl: './exam-overview.component.css'
})
export class ExamOverviewComponent {
  exam: IGetExamByIdResponse | null = null;
  currentQuestionIndex: number = 0;
  selectedAnswers: { [questionId: number]: number[] } = {}; 

  constructor(private examClient: ExamClient, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    const examId = Number(this.route.snapshot.paramMap.get('id'));
    if(examId){
    this.fetchExam(examId);
    }
  }

  fetchExam(id: number): void {
    this.examClient.getExamById(id).subscribe(
      (response: IGetExamByIdResponse) => {
        this.exam = response;
      },
    );
  }

  onAnswerSelected(event: { questionId: number, answerId: number, isRadio: boolean }): void {
    const { questionId, answerId, isRadio } = event;
    
    if (isRadio) {
      this.selectedAnswers[questionId] = [answerId];
    } else {
      const selectedAnswersForQuestion = this.selectedAnswers[questionId] || [];
      const index = selectedAnswersForQuestion.indexOf(answerId);
      if (index > -1) {
        selectedAnswersForQuestion.splice(index, 1);
      } else {
        selectedAnswersForQuestion.push(answerId);
      }
      this.selectedAnswers[questionId] = selectedAnswersForQuestion;
    }
  }

  submitExam(): void {
  const submitCommand = new SubmitExamCommand({
    examId: this.exam!.id,
    examQuestions: this.exam!.questions?.map(q => new SubmitExamExamQuestionDto({
      id: q.id!,
      submittedAnswers: this.selectedAnswers[q.id!] || [] 
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

  get totalQuestions(): number {
    return this.exam?.questions?.length ?? 0;
  }

}