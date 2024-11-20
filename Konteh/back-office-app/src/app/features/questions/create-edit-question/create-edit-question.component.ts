import { Component } from '@angular/core';
import { FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { CreateOrUpdateQuestionAnswerRequest, CreateOrUpdateQuestionQuestionRequest, GetQuestionByIdResponse, QuestionCategory, QuestionsClient, QuestionType } from '../../../api/api-reference';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionForm } from './models/question-form.model';

@Component({
  selector: 'app-create-edit-question',
  templateUrl: './create-edit-question.component.html',
  styleUrl: './create-edit-question.component.css'
})
export class CreateEditQuestionComponent {
  id: number | undefined;

  questionForm = new QuestionForm();

  constructor(private route: ActivatedRoute, private questionsClient: QuestionsClient, private router: Router) { }

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));

    if (this.id) {
      this.loadQuestion(this.id);
    }
  }

  get answers(): FormArray {
    return this.questionForm.get('answers') as FormArray;
  }

  loadQuestion(id: number): void {
    this.questionsClient.getQuestionById(id).subscribe(
      question => {
        this.questionForm.patchValue({
          id: id,
          text: question.text,
          category: question.category,
          type: question.type
        });

        question.answers?.forEach(answer => {
          this.answers.push(new FormGroup({
            id: new FormControl(answer.id),
            text: new FormControl(answer.text, [Validators.required]),
            isCorrect: new FormControl(answer.isCorrect)
          }));
        });
      }
    );
  }

  onSubmit(): void {
    if (this.questionForm.valid) {
      const request = this.formRequest();
      this.createOrEditQuestion(request);
    }
  }

  formRequest(): CreateOrUpdateQuestionQuestionRequest {
    const questionData = this.questionForm.value;

    const request: CreateOrUpdateQuestionQuestionRequest = new CreateOrUpdateQuestionQuestionRequest({
      id: questionData.id,
      text: questionData.text!,
      category: questionData.category!,
      type: questionData.type!,
      answers: questionData.answers!.map((answer: { id: number | undefined, text: string, isCorrect: boolean }) =>
        new CreateOrUpdateQuestionAnswerRequest({
          id: answer.id,
          text: answer.text,
          isCorrect: answer.isCorrect
        })
      )
    });

    return request;
  }

  createOrEditQuestion(request: CreateOrUpdateQuestionQuestionRequest): void {
    this.questionsClient.createOrUpdate(request).subscribe(_ => this.router.navigate(['questions']));
  }
}
