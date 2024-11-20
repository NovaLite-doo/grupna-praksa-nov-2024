import { FormControl, FormGroup, Validators } from "@angular/forms";

export class AnswerForm extends FormGroup {
    constructor() {
        super({
            id: new FormControl<string | undefined>(undefined),
            text: new FormControl<string | undefined>(undefined, Validators.required),
            isCorrect: new FormControl<boolean>(false)
        })
    }
}