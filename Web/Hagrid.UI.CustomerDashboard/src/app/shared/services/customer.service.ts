import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/observable/defer";
import { AuthService } from "../auth/auth.service";

@Injectable()
export class CustomerService {
  constructor(private http: HttpClient, private authService: AuthService) {}

  public get(token: string): Observable<any> {
    let headers = new HttpHeaders({ Authorization: token });

    return this.http.get("/member/", { headers });
  }

  public getAddresses(): Observable<any> {
    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token
    });

    return this.http.get("/member/" + account.code + "/addresses/", {
      headers
    });
  }

  public deleteAddress(code: string): Observable<any> {

    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token
    });

    return this.http.delete("/member/" + account.code + "/addresses/" + code, { headers });
  }

  public saveAddress(address: any): Observable<any> {

    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token
    });

    return this.http.post("/member/" + account.code + "/addresses/", address, { headers });
  }

  public getAddress(code: string): Observable<any> {
    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token
    });

    return this.http.get("/member/" + account.code + "/addresses/" + code, { headers });
  }
}
