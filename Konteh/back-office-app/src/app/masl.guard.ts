import { MsalService } from '@azure/msal-angular';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class MaslGuard implements CanActivate {
  constructor(private authService: MsalService) {

  }

  canActivate(
    _route: ActivatedRouteSnapshot,
    _state: RouterStateSnapshot) {
    return this.authService.instance.getActiveAccount() !== null;
  }
}