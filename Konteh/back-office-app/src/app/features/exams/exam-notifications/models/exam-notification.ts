import { CandidateNotification } from "./candidate-notification";

export class ExamNotification {
    id: number = -1;
    text: string = '';
    isCompleted: boolean = false;
    questionCount?: number;
    correctAnswerCount?: number;
    score?: number;
    candidate?: CandidateNotification;
}