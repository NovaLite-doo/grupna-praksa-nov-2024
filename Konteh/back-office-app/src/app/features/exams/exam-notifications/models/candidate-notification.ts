import { YearOfStudy } from "../../../../api/api-reference";

export class CandidateNotification {
    name: string = '';
    surname: string = '';
    email: string = '';
    faculty: string = '';
    major: string = '';
    yearOfStudy: YearOfStudy = YearOfStudy.YearOne;
}