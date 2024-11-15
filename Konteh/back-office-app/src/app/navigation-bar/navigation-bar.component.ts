import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { AuthenticationResult } from '@azure/msal-browser';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrl: './navigation-bar.component.css'
})
export class NavigationBarComponent {
  apiResponse: string = '';

  constructor(private authService: MsalService, private httpClient: HttpClient) {

  }

  ngOnInit(): void {
    this.authService.initialize().subscribe(_ => {
      this.authService.instance.handleRedirectPromise().then(res => {
        if (res != null && res.account != null) {
          this.authService.instance.setActiveAccount(res.account)
        }
      })
    });

  }

  isLoggedIn(): boolean {
    return this.authService.instance.getActiveAccount() != null
  }

  login() {
    const loginRequest = {
      scopes: ['api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.read', 'api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.write']
    };
  
    this.authService.loginPopup(loginRequest).subscribe(
      (response: AuthenticationResult) => {
        this.authService.instance.setActiveAccount(response.account);
        console.log('User logged in successfully');
      },
      (error) => {
        console.error('Login error: ', error);
      }
    );
  }
  

  logout() {
    this.authService.logout()
  }

  testApiRequest() {
    if (!this.isLoggedIn()) {
      console.log('User is not logged in');
      return;
    }
  
    this.authService.acquireTokenSilent({
      scopes: ['api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.read']
    }).subscribe({
      next: (tokenResponse) => {
        const token = tokenResponse.accessToken;
        console.log('Access token: ', token); 
      },
      error: (err) => {
        console.error('Error acquiring token: ', err);
      }
    });
  }
  
}
