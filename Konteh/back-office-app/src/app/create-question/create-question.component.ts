import { Component } from '@angular/core';
import { FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { CreateQuestionAnswerRequest, CreateQuestionQuestionRequest, QuestionCategory, QuestionsClient, QuestionType } from '../api/api-reference';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-create-question',
  templateUrl: './create-question.component.html',
  styleUrls: ['./create-question.component.css']
})
export class CreateQuestionComponent {
  questionForm = new FormGroup({
    text: new FormControl('', [Validators.required]),
    category: new FormControl('', [Validators.required]), 
    type: new FormControl('', [Validators.required]),
    answers: new FormArray([])
  })

  constructor(private questionsClient: QuestionsClient) {}

  get answers(): FormArray {
    return this.questionForm.get('answers') as FormArray;
  }

  onSubmit(): void {
    if (this.questionForm.valid) {
      const questionData = this.questionForm.value;

      const request: CreateQuestionQuestionRequest = new CreateQuestionQuestionRequest();
      request.text = questionData.text!;
      request.category = QuestionCategory.GIT;
      request.type = QuestionType.Checkbox;

      request.answers = questionData.answers!.map((answer: { text: string, isCorrect: boolean }) => {
        return new CreateQuestionAnswerRequest({
          text: answer.text,
          isCorrect: answer.isCorrect
        });
      });

      this.questionsClient.create(request)
        .subscribe(response => {
          console.log('Question created successfully', response);
        }, (error: HttpErrorResponse) => {
          console.error('Error creating question', error);
        });
    } else {
      console.log('Form is not valid');
    }
  }
}
