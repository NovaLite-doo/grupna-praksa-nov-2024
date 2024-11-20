import { RouterModule, Routes } from "@angular/router";
import { CreateEditQuestionComponent } from "./create-edit-question/create-edit-question.component";
import { QuestionsOverviewComponent } from "./questions-overview/questions-overview.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    {
        path: '',
        component: QuestionsOverviewComponent
    },
    {
        path: 'add',
        component: CreateEditQuestionComponent
    },
    {
        path: ':id',
        component: CreateEditQuestionComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QuestionsRoutingModule { }