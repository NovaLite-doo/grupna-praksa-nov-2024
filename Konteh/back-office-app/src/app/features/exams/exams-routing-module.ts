import { RouterModule, Routes } from "@angular/router";
import { ExamsOverviewComponent } from "./exams-overview/exams-overview.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    {
        path: '',
        component: ExamsOverviewComponent
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ExamsRoutingModule { }