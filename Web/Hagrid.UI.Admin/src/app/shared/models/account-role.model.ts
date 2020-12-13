import {Account} from "./account.model";
import { Deserializable } from "./intefaces/deserializable.model";

export class AccountRole implements Deserializable {
	code: string;
	account: Account;

	constructor();
	constructor(code: string, account: Account);
	constructor(code?: string, account?: Account) {

		if (code !== undefined)
			this.code = code;
		if(account !== undefined)
			this.account = account;
	}

	deserialize(input: any): this {
		Object.assign(this, input);
		this.account = new Account().deserialize(input.account);
		this.code = input.code;
		return this;
	}
}
