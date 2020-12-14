import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class StoresService {

    constructor(private http: HttpClient) { }

    public get(id: string): Observable<any> {
        return this.http.get(`/store/${id}`);
    }

    public getApplications(): Observable<any> {
        return this.http.get('/application');
    }

    public getByTerm(term: any): Observable<any> {
        return this.http.get(`/store/${term}/by-term`, term);
    }

    public search(filters: any): Observable<any> {
        return this.http.post('/store/search', filters);
    }

    public uploadLogo(code: string, file: File): Observable<any> {
        return this.http.post(`/store/upload/${code}`, file);
    }

    public update(store: any){
        return this.http.put('/store/', store);
    }

    public createStore(store: any){
      return this.http.post('/store/', store);
  }

    public getLinkEncripted(id: string, secretPhrase: string): Observable<any>{
        return this.http.get(`/store/encripted/${id}/${secretPhrase}`);
	}

	public getApplicationStore(code: string): Observable<any> {
        return this.http.get(`/application-store/store/${code}`);
	}
}
