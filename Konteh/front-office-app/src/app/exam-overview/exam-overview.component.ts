import { Component } from '@angular/core';
import { Answer, ExamClient, IGetExamByIdResponse, ISubmitExamCommand, SubmitExamCommand, SubmitExamExamQuestionDTO } from '../api/api-reference';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-exam-overview',
  templateUrl: './exam-overview.component.html',
  styleUrl: './exam-overview.component.css'
})
export class ExamOverviewComponent {

  exam: IGetExamByIdResponse | null = null;
  currentQuestionIndex: number = 0;
  selectedAnswers: { [questionId: number]: Answer[] } = {};

  constructor(private examClient: ExamClient, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    const examId = Number(this.route.snapshot.paramMap.get('id'));
    this.fetchExam(examId);
  }

  fetchExam(id: number): void {
    this.examClient.getExamById(id).subscribe(
      (response: IGetExamByIdResponse) => {
        this.exam = response;
        if (this.exam?.questions) {
          this.exam.questions.forEach(q => {
            if (!q.questionId) {
              console.error(`Question with missing ID:`, q);
            }
            if (!q.answers) {
              console.error(`No answers available for question with ID:`, q.questionId);
            }
          });
        }
      },
      (error) => {
        console.error('Error fetching exam:', error);
      }
    );
  }

  onAnswerSelected(questionId: number, answer: Answer): void {
    if (!this.selectedAnswers[questionId]) {
      this.selectedAnswers[questionId] = [];
    }

    
    const index = this.selectedAnswers[questionId].indexOf(answer);
    if (index === -1) {
      this.selectedAnswers[questionId].push(answer);
    } else {
      this.selectedAnswers[questionId].splice(index, 1);
    }
  }

  isAnswerSelected(questionId: number, answerId: number): boolean {
    const selectedAnswersForQuestion = this.selectedAnswers[questionId];
    if (!selectedAnswersForQuestion) return false;
  
    if (this.exam?.questions?.find(q => q.id === questionId)?.type === 0) {
      return selectedAnswersForQuestion.length > 0 && selectedAnswersForQuestion[0].id === answerId;
    }

    return selectedAnswersForQuestion.some(a => a.id === answerId);
  }
  

  submitExam(): void {
    if (!this.exam) {
      console.error('No exam available to submit.');
      return;
    }
 
    const submitCommand = new SubmitExamCommand({
      examId: this.exam.id,
      examQuestions: this.exam?.questions?.map(q => {
          const submitExamQuestion = new SubmitExamExamQuestionDTO();
          submitExamQuestion.id = q.id!;
          submitExamQuestion.submittedAnswers = this.selectedAnswers[q.id!] || [];
          return submitExamQuestion;
      })
  });
  
  submitCommand.init();
  
    console.log('Submit Exam Command:', submitCommand);
  
    this.examClient.submitExam(submitCommand).subscribe(
      () => {
        console.log('Exam submitted successfully');
        alert('Your exam has been submitted successfully.');
        this.resetExamState();
        this.router.navigate(['/']); 
      },
      (error) => {
        console.error('Error submitting exam:', error);
        alert('There was an error submitting your exam. Please try again.');
      }
    );
  }

  resetExamState(): void {
    this.selectedAnswers = {}; 
    this.currentQuestionIndex = 0; 
    this.exam = null; 
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
