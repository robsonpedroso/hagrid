import { Component, OnInit, AfterContentInit } from '@angular/core';
import { AuthService } from '../../shared/auth/auth.service';
import { environment } from '../../../environments/environment';

@Component({
	selector: 'app-master-header',
	templateUrl: './master-header.component.html'
})
export class MasterHeaderComponent implements OnInit, AfterContentInit {

	public account: any = {};
	public is_manager: boolean = false;

	constructor(private authService: AuthService) {
		this.is_manager = environment.is_manager;
	}

	public ngOnInit() {

		this.authService.getAccountAsync().subscribe((account) => {
			this.account = account;
		});
	}

	public ngAfterContentInit() {
		if (!this.account.code) {
			let account = this.authService.getAccount();
			if (account) {
				this.account = account
			}
		}
	}

	public logout() {
		this.authService.logout();
	}
}
