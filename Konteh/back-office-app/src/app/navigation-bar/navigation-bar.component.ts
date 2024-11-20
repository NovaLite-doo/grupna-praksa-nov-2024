import { Component } from '@angular/core';
import { MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrl: './navigation-bar.component.css'
})
export class NavigationBarComponent {

  constructor(private authService: MsalService) {

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
    this.authService.loginPopup().subscribe(
      response => this.authService.instance.setActiveAccount(response.account)
    );
  }

  logout() {
    this.authService.logout()
  }
}