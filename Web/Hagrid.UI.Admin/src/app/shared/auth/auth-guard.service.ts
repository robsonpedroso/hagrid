import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';
import { SessionStorageService } from 'ngx-webstorage';
import { AccountService } from '../../accounts/accounts.service';
import { Keys } from '../models/keys';

@Injectable()
export class AuthGuardService implements CanActivate {

	private static get STATE_KEY(): string { return 'State'; };

	constructor(
		private authService: AuthService,
		private router: Router,
		private sessionStorageService: SessionStorageService,
		private accountService: AccountService) {
	}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

		if (route.data.type === 'authorized') {

			let transfer_token = route.params.transfer_token;

			this.authService.authenticate(transfer_token).subscribe((response) => {

				this.sessionStorageService.store(AuthService.ACCOUNT_AUTH_KEY, response);

				this.accountService.getRole().subscribe((r) => {
					
					this.sessionStorageService.store(Keys.StoragesRole, r);

					let stateUrl = this.sessionStorageService.retrieve(AuthGuardService.STATE_KEY);

					if (stateUrl) {
						this.router.navigate([stateUrl]);
						this.sessionStorageService.clear(AuthGuardService.STATE_KEY);
					}
					else {
						this.router.navigate(['/']);
					}

					return true;
				});

			});
		}
		else {
			let isAuthenticated = this.authService.isAuthenticated();

			if (isAuthenticated) {
				return true;
			} else {
				this.sessionStorageService.store(AuthGuardService.STATE_KEY, state.url);
				this.authService.redirectLoginPage();
				return false;
			}
		}
	}
}
