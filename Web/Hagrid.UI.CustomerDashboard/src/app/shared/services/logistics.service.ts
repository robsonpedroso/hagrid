import { Injectable } from '@angular/core';
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from '../auth/auth.service';
import { Observable } from 'rxjs/Observable';
import { Http ,HttpModule} from '@angular/http'

@Injectable()
export class LogisticsService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  public getAddress(zipCode: string): Observable<any> {

    let account = this.authService.getAccount();

    let headers = new HttpHeaders({
      Authorization: account.token.access_token
    });

    return this.http.get(environment.urls.logisticsEndpoint + "/zipcode/" + zipCode, { headers });
  }

}
