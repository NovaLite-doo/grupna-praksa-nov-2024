import { FormArray, FormControl, FormGroup, Validators } from "@angular/forms";
import { QuestionCategory, QuestionType } from "../../../../api/api-reference";
import { AnswerForm } from "./answer-form.model";

export class QuestionForm extends FormGroup {
    constructor() {
        super({
            id: new FormControl(),
            text: new FormControl('', [Validators.required]),
            category: new FormControl<QuestionCategory | null>(null, [Validators.required]),
            type: new FormControl<QuestionType | null>(null, [Validators.required]),
            answers: new FormArray<AnswerForm>([])
        })
    }
}