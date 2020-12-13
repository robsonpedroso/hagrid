import { AuthService } from './../auth/auth.service';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { SessionStorageService } from 'ngx-webstorage';
import 'rxjs/add/observable/defer';

@Injectable()
export class SuperpointsService {

  constructor(
    private sessionStorageService: SessionStorageService,
    private http: HttpClient,
    private authService: AuthService) { }


  public getBalance(): Observable<any> {

    let account = this.authService.getAccount();

    let headers = new HttpHeaders({ 'Authorization': account.token.access_token });

    return this.http.get('/member/points/balance/', { headers } );
  }

  public getExtract(extract: any): Observable<any> {

    let account = this.authService.getAccount();

    let headers = new HttpHeaders({ 'Authorization': account.token.access_token, 'Content-Type': 'application/json'});

    return this.http.post('/member/points/extract', extract, { headers } );
  }

}
