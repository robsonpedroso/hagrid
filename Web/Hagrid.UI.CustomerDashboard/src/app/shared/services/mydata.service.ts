import { AuthService } from "./../auth/auth.service";
import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { SessionStorageService } from "ngx-webstorage";
import "rxjs/add/observable/defer";

@Injectable()
export class MyDataService {
  constructor(
    private sessionStorageService: SessionStorageService,
    private http: HttpClient,
    private authService: AuthService
  ) {}

  public get(): Observable<any> {
    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token
    });

    return this.http.get("/member/", { headers });
  }

  public update(acc: any): any {
    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token,
      "Content-Type": "application/json"
    });

    return this.http.put("/member/", acc, { headers });
  }

  public async changePassword(password: string, password_new: string) {
    let account = this.authService.getAccount();
    let firstToken: any;
    let token: any;

    if (account) {
      let headers = new HttpHeaders({Authorization: account.token.access_token, "Content-Type": "application/json"});


      let body = `url_back=https://accounts.genstore.com.br/customer-dashboard`;

      firstToken = await this.http
        .post("/member/password-change/token", body, { headers }).toPromise();

      headers = new HttpHeaders({ "Content-Type": "application/json" });
      token = await this.http.post(`/customer-dashboard/change-password/token`, {token: firstToken.token}, { headers }).toPromise();

      headers = new HttpHeaders({ "Content-Type": "application/json" });
      let data = {token:  token.access_token, password: password, password_new: password_new };
      return this.http.post("/customer-dashboard/change-password", data, { headers }).toPromise();
    }
  }

  public post(creditcard: any): Observable<any> {
    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token,
      "Content-Type": "application/json"
    });

    return this.http.post("/member/credit-cards/insert", creditcard, {
      headers
    });
  }

  public delete(code: string): Observable<any> {
    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token,
      "Content-Type": "application/json"
    });

    return this.http.delete("/member/credit-cards/" + code, { headers });
  }
}
