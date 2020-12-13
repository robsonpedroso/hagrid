import { Deserializable } from "./intefaces/deserializable.model";

export class Store implements Deserializable {
	code: string;
	name: string;

	constructor();
	constructor(code: string, name: string);
	constructor(code?: string, name?: string) {

		if(this.code !== undefined)
			this.code = code;

		if(this.name !== undefined)
			this.name = name;
	}

	deserialize(input: any): this {
		Object.assign(this, input);
		this.code = input.code;
		this.name = input.name;
		return this;
	}
}
