import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SessionStorageService } from 'ngx-webstorage';
import 'rxjs/add/operator/do';
import 'rxjs/add/observable/defer';
import { Keys } from '../models/keys';

@Injectable()
export class AuthService {

    constructor(
        private http: HttpClient,
        private sessionStorageService: SessionStorageService
    ) { }

	public static get ACCOUNT_AUTH_KEY(): string { return 'Auth'; };


    public getAccount(): any {
        return this.sessionStorageService.retrieve(AuthService.ACCOUNT_AUTH_KEY);
    };

    public getAccountAsync(): Observable<any> {
        return this.sessionStorageService.observe(AuthService.ACCOUNT_AUTH_KEY);
    };

    public authenticate(transfer_token: string): Observable<any> {

        let headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

        return this.http.get(`/auth/${transfer_token}`, { headers });
    }

    public isAuthenticated(): boolean {

        let account = this.getAccount();

        if (account)
            return true;
        else
            return false;
    }

    public refreshToken(): Observable<any> {

        let account = this.getAccount();

        if (account) {
            let headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
            let body = `grant_type=refresh_token&refresh_token=${account.token.refresh_token}&client_id=${environment.client_id}`;

            return this.http.post('/token', body, { headers });
        }
    }

    public redirectLoginPage(): void {

        let url = `${environment.urls.accountSiteURL}/#/login/?ub=${environment.urls.accountAdminURL}/authorized/`;

        window.open(url, '_self');
    }

    public logout(): void {

		this.sessionStorageService.clear(AuthService.ACCOUNT_AUTH_KEY);
		this.sessionStorageService.clear(Keys.StoragesRole);

        this.redirectLoginPage();
    }
}
