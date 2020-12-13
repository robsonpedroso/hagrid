import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from '../auth/auth.service';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class OrdersService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  public getOrders(search: any): Observable<any> {
    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token
    });

    return this.http.post("/member/orders", search, { headers });
  }

}
