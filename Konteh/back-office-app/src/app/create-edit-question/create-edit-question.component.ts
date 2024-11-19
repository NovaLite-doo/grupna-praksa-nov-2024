import { Component } from '@angular/core';
import { FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { CreateOrUpdateQuestionAnswerRequest, CreateOrUpdateQuestionQuestionRequest, GetQuestionByIdResponse, QuestionCategory, QuestionsClient, QuestionType } from '../api/api-reference';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-create-edit-question',
  templateUrl: './create-edit-question.component.html',
  styleUrl: './create-edit-question.component.css'
})
export class CreateEditQuestionComponent {
  id: number | undefined;

  questionForm = new FormGroup({
    id: new FormControl(),
    text: new FormControl( '', [Validators.required]),
    category: new FormControl<QuestionCategory | null>(null, [Validators.required]), 
    type: new FormControl<QuestionType | null>(null, [Validators.required]),
    answers: new FormArray([])
  })

  constructor(private route: ActivatedRoute, private questionsClient: QuestionsClient) {}

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
      (question: GetQuestionByIdResponse) => {
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
      },
      (error: HttpErrorResponse) => {}
    );
  }

  onSubmit(): void {
    if(this.questionForm.valid) {
      const request = this.formRequest();

      if(this.id) {
        this.editQuestion(request);
      } else {
        this.createQuestion(request);
      }
    }
  }

  formRequest(): CreateOrUpdateQuestionQuestionRequest {
    const questionData = this.questionForm.value;

    console.log(questionData)

    const request: CreateOrUpdateQuestionQuestionRequest = new CreateOrUpdateQuestionQuestionRequest();
    request.id = questionData.id;
    request.text = questionData.text!;
    request.category = questionData.category ? questionData.category : QuestionCategory.OOP;
    request.type = questionData.type ? questionData.type : QuestionType.Radiobutton;

    request.answers = questionData.answers!.map((answer: { id: number | undefined, text: string, isCorrect: boolean }) => {
      return new CreateOrUpdateQuestionAnswerRequest({
        id: answer.id,
        text: answer.text,
        isCorrect: answer.isCorrect
      });
    });

    console.log(request)

    return request;
  }

  createQuestion(request: CreateOrUpdateQuestionQuestionRequest): void {
    this.questionsClient.createOrUpdate(request).subscribe(
      (response) => {}, 
      (error: HttpErrorResponse) => {}
    );
  }

  editQuestion(request: CreateOrUpdateQuestionQuestionRequest) {
    this.questionsClient.createOrUpdate(request).subscribe(
      (response) => {}, 
      (error: HttpErrorResponse) => {}
    );
  }
}
