import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { environment } from "../../../environments/environment";

@Injectable()
export class LogisticsService {

    constructor(private http: HttpClient) { }

    public getAddressByZipCode(zipCode: string): Observable<any> {
      var url = environment.urls.logisticsEndpoint+`/zipcode/${zipCode}`;
      return this.http.get(url);
    }
}
