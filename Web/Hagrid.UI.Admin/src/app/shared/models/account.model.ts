import { Deserializable } from "./intefaces/deserializable.model";

export class Account implements Deserializable {
	code: string;
	email: string;
	document: string;

	public constructor();
	public constructor(code: string);
	public constructor(code: string, email: string, document: string);
	public constructor(code?: string, email?: string, document?: string){

		if(code !== undefined)
			this.code = code;

		if(email !== undefined)
			this.email = email;

		if(document !== undefined)
			this.document = document
	}

	deserialize(input: any): this {
		Object.assign(this, input);
		this.code = input.code;
		this.email = input.email;
		this.document = input.document;

		return this;
	}
}
