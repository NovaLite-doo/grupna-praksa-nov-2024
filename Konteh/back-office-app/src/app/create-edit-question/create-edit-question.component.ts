import { Component } from '@angular/core';
import { FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { CreateQuestionAnswerRequest, CreateQuestionQuestionRequest, EditQuestionAnswerRequest, EditQuestionNewAnswerRequest, EditQuestionQuestionRequest, GetQuestionByIdResponse, QuestionCategory, QuestionsClient, QuestionType } from '../api/api-reference';
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

  loadQuestion(id: number): void {
    this.questionsClient.getQuestionById(id).subscribe(
      (question: GetQuestionByIdResponse) => {
        this.questionForm.patchValue({
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
      (error: HttpErrorResponse) => {
        console.error('Error loading question', error);
      }
    );
  }

  get answers(): FormArray {
    return this.questionForm.get('answers') as FormArray;
  }

  onSubmit(): void {
    if(this.questionForm.valid) {
      if(this.id) {
        this.editQuestion();
      } else {
        this.createQuestion();
      }
    } else {
      console.log('Form is not valid');
    }
  }

  createQuestion(): void {
    const questionData = this.questionForm.value;

    const request: CreateQuestionQuestionRequest = new CreateQuestionQuestionRequest();
    request.text = questionData.text!;
    request.category = questionData.category ? questionData.category : QuestionCategory.GIT;
    request.type = questionData.type ? questionData.type : QuestionType.Checkbox;

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
  }

  editQuestion() {
    const questionData = this.questionForm.value;

    const request: EditQuestionQuestionRequest = new EditQuestionQuestionRequest();
    request.id = this.id!;
    request.text = questionData.text!;
    request.category = questionData.category ? questionData.category : QuestionCategory.OOP;
    request.type = questionData.type ? questionData.type : QuestionType.Radiobutton;

    request.answers = questionData.answers!
      .filter((answer: { id: number | null, text: string, isCorrect: boolean }) => answer.id !== null)
      .map((answer: { id: number, text: string, isCorrect: boolean }) => {
        return new EditQuestionAnswerRequest({
          id: answer.id,
          text: answer.text,
          isCorrect: answer.isCorrect
        });
    });

    request.newAnswers = questionData.answers!
      .filter((answer: { id: number | null, text: string, isCorrect: boolean }) => answer.id === null)
      .map((answer: { text: string, isCorrect: boolean }) => {
        return new EditQuestionNewAnswerRequest({
          text: answer.text,
          isCorrect: answer.isCorrect
        });
    });

    this.questionsClient.edit(request)
      .subscribe(response => {
        console.log('Question edited successfully', response);
      }, (error: HttpErrorResponse) => {
        console.error('Error edited question', error);
    });
  }
}
