import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Role } from '../shared/models/role.model';
@Injectable()
export class PermissionService {

	constructor(private http: HttpClient) { }

	//#region " Resources "
	public getResources(filter: any): Observable<any> {
		let name = filter.name;
		let application_code = filter.application_code;
		let skip = filter.skip;
		let take = filter.take;

		return this.http.get(`/applications/resources?name=${name}&application_code=${application_code}&skip=${skip}&take=${take}`);
	}

	public removeResources(code: string): Observable<any> {
		return this.http.delete(`/applications/resources/${code}`);
	}

	public saveResources(resources: any): Observable<any> {
		return this.http.post(`/applications/resources`, resources);
	}

	//#endregion

	//#region " Roles "

	public getRoles(filter: any): Observable<any> {

		let name = filter.name !== undefined ? filter.name : "";
		let store_code = filter.storePermission !== undefined ? filter.storePermission : "00000000-0000-0000-0000-000000000000";
		let skip = filter.skip;
		let take = filter.take;

		return this.http.get(`/roles?store_code=${store_code}&name=${name}&skip=${skip}&take=${take}`);
	}

	public removeRole(code: string): Observable<any> {
		return this.http.delete(`/roles/${code}`);
	}

	public unlinkAccount(codeAccount: string, codeRole: string): Observable<any> {
		return this.http.delete(`/member/${codeAccount}/roles/${codeRole}`);
	}

	public get(id: string): Observable<Role> {

		return this.http.get(`/roles/${id}/detail`)
			.map((res: Response) => {
				return new Role().deserialize(res);
			});
	}

	public saveRole(role: any): Observable<any> {
		return this.http.post(`/roles`, role);

	}

	//#endregion

}
