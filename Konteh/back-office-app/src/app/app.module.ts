import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';

import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { InteractionType, IPublicClientApplication, PublicClientApplication } from '@azure/msal-browser';
import { MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG, MsalInterceptor, MsalInterceptorConfiguration, MsalModule, MsalService, MsalBroadcastService  } from '@azure/msal-angular';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { environment } from '../enviroments/enviroment';
import { MatFormFieldModule } from '@angular/material/form-field'; 
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { PublicPageComponent } from './public-page/public-page.component';
import { RestrictedPageComponent } from './restricted-page/restricted-page.component';
import { QuestionFormComponent } from './question-form/question-form.component';
import { ConfirmationDialogComponent } from './shared/confirmation-dialog/confirmation-dialog.component';
import { CreateEditQuestionComponent } from './create-edit-question/create-edit-question.component';
import { AnswerFormComponent } from './answer-form/answer-form.component';


export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: environment.msalConfig.clientId,  
      authority: environment.msalConfig.authority,  
      redirectUri: environment.msalConfig.redirectUri
    }
  });
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  protectedResourceMap.set('https://localhost:7285', [
    'api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.read',
    'api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.write'
  ]);

  return {
    interactionType: InteractionType.Popup,
    protectedResourceMap
  };
}

@NgModule({
  declarations: [
    AppComponent,
    QuestionFormComponent,
    ConfirmationDialogComponent,
    CreateEditQuestionComponent,
    NavigationBarComponent,
    PublicPageComponent,
    RestrictedPageComponent,
    AnswerFormComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MsalModule, 
    MatToolbarModule,
  ],
  providers: [
    provideAnimationsAsync(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    }, {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory
    }, {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MSALInterceptorConfigFactory
    },
    MsalService,
    MsalBroadcastService  
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }