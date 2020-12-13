import { AuthService } from './../auth/auth.service';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { SessionStorageService } from 'ngx-webstorage';
import 'rxjs/add/observable/defer';

@Injectable()
export class CardsService {

  constructor(
    private sessionStorageService: SessionStorageService,
    private http: HttpClient,
    private authService: AuthService) { }


  public get(): Observable<any> {

    let account = this.authService.getAccount();

    let headers = new HttpHeaders({ 'Authorization': account.token.access_token });

    return this.http.get('/member/credit-cards/list', { headers } );
  }

  public post(creditcard: any): Observable<any> {

    let account = this.authService.getAccount();

    let headers = new HttpHeaders({ 'Authorization': account.token.access_token, 'Content-Type': 'application/json'});

    return this.http.post('/member/credit-cards/insert', creditcard, { headers } );
  }

  public delete(code: string): Observable<any> {

    let account = this.authService.getAccount();

    let headers = new HttpHeaders({ 'Authorization': account.token.access_token, 'Content-Type': 'application/json'});

    return this.http.delete('/member/credit-cards/' + code, { headers });
}

}
