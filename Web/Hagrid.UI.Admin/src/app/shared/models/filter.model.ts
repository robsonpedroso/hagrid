
export class Filter {
	code: string;
	application_name: string;
	application_code: string;
	email: string;
	document: string;
	name: string;
	skip: number;
	take: number;

	constructor()
	constructor(code: string, application_name: string, application_code: string, name: string, skip: number, take: number, email: string, document: string)
	constructor(code?: string, application_name?: string, application_code?: string, name?: string, skip?: number, take?: number, email?: string, document?: string) {

		this.code = code !== undefined ? code : '';
		this.email = email !== undefined ? email : '';
		this.name = name !== undefined ? name : '';
		this.document = document !== undefined ? document : '';
		this.application_name  = application_name !== undefined ? application_name : '';
		this.application_code = application_code !== undefined ? application_code : '';
		this.skip = skip !== undefined ? skip : 0;
		this.take = take !== undefined ? take : 5;
	}
}
