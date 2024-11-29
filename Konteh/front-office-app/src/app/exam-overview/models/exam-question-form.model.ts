import { FormGroup, FormControl, FormArray } from "@angular/forms";
import { GetExamByIdExamQuestionDto, QuestionType } from "../../api/api-reference";
import { AnswerForm } from "./answer-form.model";

export class ExamQuestionForm extends FormGroup {
    constructor(question: GetExamByIdExamQuestionDto) {
        super({
            id: new FormControl(question.id),
            text: new FormControl(question.text),
            type: new FormControl(question.type),
            selectedAnswer: new FormControl<number | undefined>(undefined),
            answers: new FormArray<AnswerForm>([])
        });

        question.answers?.forEach(answer => (this.controls["answers"] as FormArray<AnswerForm>).push(new AnswerForm(answer)))
    }

    get selectedAnswer() {
        return this.controls["selectedAnswer"].value as number | undefined;
    }
}