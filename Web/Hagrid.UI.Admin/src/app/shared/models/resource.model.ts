import { Application } from "./application.model";
import { Deserializable } from "./intefaces/deserializable.model";

export class Resource implements Deserializable {
	code: string;
	name: string;
	operations: string;
	application: Application;

	constructor();
	constructor(code: string, name: string, operations: string, application: Application);
	constructor(code?: string, name?: string, operations?: string, application?: Application) {

		if (code !== undefined)
			this.code = code;

		if (name !== undefined)
			this.name = name;

		if (operations !== undefined)
			this.operations = operations;

		if (application !== undefined){
			this.application = application;
		}
		else{
			this.application = new Application();
		}
	}

	deserialize(input: any): this {
		Object.assign(this, input);
		this.application = new Application().deserialize(input.application);
		return this;
	}
}
