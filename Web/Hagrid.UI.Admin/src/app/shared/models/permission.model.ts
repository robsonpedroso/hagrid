import { Resource } from "./resource.model";
import { Deserializable } from "./intefaces/deserializable.model";

export class Permission implements Deserializable {
	code: string;
	operations: string;
	resource: Resource;

	constructor();
	constructor(code: string, operations: string, resource: Resource);
	constructor(code?: string, operations?: string, resource?: Resource) {

		if (code !== undefined)
			this.code = code;

		if (operations !== undefined)
			this.operations = operations;

		if(resource !== undefined){
			this.resource = resource;
		}
		else{
			this.resource = new Resource();
		}
	}

	deserialize(input: any): this {
		Object.assign(this, input);
		this.resource = new Resource().deserialize(input.resource);
		this.operations = input.operations;
		return this;
	}
}
