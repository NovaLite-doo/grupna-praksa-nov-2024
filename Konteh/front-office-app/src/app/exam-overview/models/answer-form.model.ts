import { FormGroup, FormControl } from "@angular/forms";
import { GetExamByIdAnswerDto } from "../../api/api-reference";

export class AnswerForm extends FormGroup {
    constructor(answer: GetExamByIdAnswerDto) {
        super({
            id: new FormControl(answer.id),
            text: new FormControl(answer.text),
            isSelected: new FormControl(false)
        })
    }

    get text() {
        return this.controls["text"].value as string;
    }

    get id() {
        return this.controls["id"].value as number;
    }

    get isSelected() {
        return this.controls["isSelected"];
    }
}