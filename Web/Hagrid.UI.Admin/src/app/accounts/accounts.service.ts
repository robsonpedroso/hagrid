import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class AccountService {

    constructor(private http: HttpClient) { }

    public save(account: any): Observable<any> {
        return this.http.post(`/member/save-account-admin`, account);
    }

    public get(id: string): Observable<any> {
        return this.http.get(`/member/${id}/detail`);
    }

    public search(filters: any): Observable<any> {
        return this.http.post('/member/search', filters);
    }

    public setPermission(permission: any, type: string): Observable<any> {
        return this.http.post(`/member/permission/${type}`, permission);
    }

	public changeAccountInfo(account: any): Observable<any> {
		return this.http.post('/member/info', account);
	}

	public block(data: any): Observable<any>{
		return this.http.post(`/blacklist`, data);
	}

	public unlock(data: any): Observable<any>{
		return this.http.post(`/blacklist/unlock`, data);
	}

    public delete(code: string): Observable<any> {
        return this.http.delete(`/member/${code}`);
    }

    public sendPasswordEmail(email: string): Observable<any> {
        return this.http.post('/member/send-password-email', { email: email });
    }

    public unlockUser(code: string): Observable<any> {
        return this.http.get(`/member/${code}/unlock-user`);
    }

    public saveDBRequisition(requisition: any): Observable<any> {
        return this.http.post('/requisition/internal', requisition);
    }

    public saveFileRequisition(formData: FormData): Observable<any> {
        return this.http.post('/requisition/with-file', formData);
    }

    public getRequisitions(storeCode: string): Observable<any> {
        return this.http.get(`/requisition/${storeCode}`);
    }

    public deleteRequisition(code: string): Observable<any> {
        return this.http.delete(`/requisition/${code}`);
	}

	public getRegistry(document: string, birthdate: string =""): Observable<any> {
        return this.http.get(`/member/registries/${document}/${birthdate}`);
	}

	public getSms(code: string): Observable<any> {
		return this.http.get(`/member/sms/sended/${code}`);
	}

	public getRole(): Observable<any>{
		return this.http.get(`/member/roles`);
	}

	public downloadCsv(code: string) {
		return this.http.get(`/requisition/csv/${code}`);
	}

}
