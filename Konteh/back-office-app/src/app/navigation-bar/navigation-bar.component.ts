import { Component } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { AuthenticationResult } from '@azure/msal-browser';
import { WeatherForecastClient } from '../api/api-reference';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrl: './navigation-bar.component.css'
})
export class NavigationBarComponent {

  constructor(private authService: MsalService, private weatherForecestClient: WeatherForecastClient) {

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

  test() {
    this.weatherForecestClient.get().subscribe(
      (data) => {
        
      },
      (error) => {
        console.log('Error loading weather forecast');
      }
    )
  }
}