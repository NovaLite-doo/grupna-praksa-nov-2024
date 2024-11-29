import { FormArray, FormControl, FormGroup } from "@angular/forms";
import { ExamQuestionForm } from "./exam-question-form.model";

export class ExamForm extends FormGroup {
    constructor() {
        super({
            id: new FormControl<number | undefined>(undefined),
            questions: new FormArray<ExamQuestionForm>([])
        });
    }

    get questions() {
        return this.controls["questions"] as FormArray<ExamQuestionForm>;
    }
}