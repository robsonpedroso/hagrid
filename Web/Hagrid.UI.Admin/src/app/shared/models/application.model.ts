import { Deserializable } from "./intefaces/deserializable.model";

export class Application implements Deserializable{
	code: string;
	name: string;

	constructor();
	constructor(code: string, name: string);
	constructor(code?: string, name?: string) {

		if (code !== undefined)
			this.code = code;

		if(name !== undefined)
			this.name = name;
	}

	deserialize(input: any): this {
		Object.assign(this, input);
		this.code = input.code;
		this.name = input.name;
		return this;
	}
}
