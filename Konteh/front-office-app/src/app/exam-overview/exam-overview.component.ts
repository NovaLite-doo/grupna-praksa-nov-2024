import { Component } from '@angular/core';
import { ExamClient, IGetExamByIdResponse } from '../api/api-reference';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-exam-overview',
  templateUrl: './exam-overview.component.html',
  styleUrl: './exam-overview.component.css'
})
export class ExamOverviewComponent {

  exam: IGetExamByIdResponse | null = null;

  constructor(private examClient: ExamClient, private route: ActivatedRoute) {}

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
            if (!q.question?.id) {
              console.error(`Question with missing ID:`, q);
            }
            if (!q.question?.answers) {
              console.error(`No answers available for question with ID:`, q.question?.id);
            }
          });
        }
      },
      (error) => {
        console.error('Error fetching exam:', error);
      }
    );
  }
}
