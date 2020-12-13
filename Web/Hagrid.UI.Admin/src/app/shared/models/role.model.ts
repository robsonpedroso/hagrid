import { AccountRole } from "./account-role.model";
import { Permission } from "./permission.model";
import { Store } from "./store.model";
import { Deserializable } from "./intefaces/deserializable.model";

export class Role implements Deserializable {
	code: string;
	name: string;
	description: string;
	account_roles: Array<AccountRole>;
	permissions: Array<Permission>;
	store: Store;

	constructor();
	constructor(code: string, name: string, description: string, account_roles: any[], permissions: any[], store: Store);
	constructor(code?: string, name?: string, description?: string, account_roles?: any[], permissions?: any[], store?: Store) {

			if (code !== undefined)
				this.code = code;

			if (name !== undefined)
				this.name = name;

			if (description !== undefined)
				this.description = description;

			if (account_roles !== undefined){
				this.account_roles = account_roles;
			}
			else{
				this.account_roles = new Array<AccountRole>();
			}

			if (permissions !== undefined){
				this.permissions = permissions;
			}
			else{
				this.permissions = new Array<Permission>();
			}

			if(store !== undefined){
				this.store = store;
			}
			else{
				this.store = new Store();
			}

	}

	deserialize(input: any): this {
		Object.assign(this, input);

		let _account_roles = new Array<AccountRole>();
		input.account_roles.forEach(item => {
			_account_roles.push(new AccountRole().deserialize(item));
		});
		this.account_roles = _account_roles;

		let _permissions = new Array<Permission>();
		input.permissions.forEach(item => {
			_permissions.push(new Permission().deserialize(item));
		});
		this.permissions = _permissions;

		this.store = new Store().deserialize(input.store);

		return this;
	}
}
