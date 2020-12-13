import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { PermissionService } from '../../permissions.service';
import { NotificationsService } from 'angular2-notifications';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../../accounts/accounts.service';
import { StoresService } from '../../../stores/stores.service';
import { Role } from '../../../shared/models/role.model';
import { Permission } from '../../../shared/models/permission.model';
import { Account } from '../../../shared/models/account.model';
import { AccountRole } from '../../../shared/models/account-role.model';
import { Resource } from '../../../shared/models/resource.model';
import { Application } from '../../../shared/models/application.model';
import { Store } from '../../../shared/models/store.model';
import { Filter } from '../../../shared/models/filter.model';
import { SelectComponent } from 'ng2-select';
import { AuthPermissionService } from '../../../shared/auth/auth-permission.service';
import { Keys } from '../../../shared/models/keys';

@Component({
	selector: 'app-permission-role-create',
	templateUrl: './permission-role-create.component.html',
	styles: []
})

export class PermissionRoleCreateComponent implements OnInit {

	@ViewChild('selectStore') selectStore: SelectComponent;
	@ViewChild('selectApplication') selectApplication: SelectComponent;

	is_loaded: boolean;
	showModalAccount: boolean;
	showModalResource: boolean;
	viewAccount: boolean;
	viewResource: boolean;
	disabledSave: boolean = false;
	disableApplication: boolean = true;
	disableButton: boolean;
	total_result_resource: any;
	filter: any = {
		skip: 0,
		take: 5
	};
	accounts_list: any[] = [];
	Resources_list: any[];
	storeSelected: any = {};
	operations: any[] = ["View", "Insert", "Edit", "Remove", "Approval"];

	role = new Role();
	permission: Permission[];
	accountRole: AccountRole[];
	store: Store;
	account: Account;
	roleFilter = new Filter();

	alert: any = {
		timeOut: 650,
		showProgressBar: true,
		pauseOnHover: true,
		clickToClose: true,
		animate: 'fromLeft'
	};
	Permission: any = {};
	Groups: any = {};
	isDisabled: boolean;

	ngOnInit() {
		this.Permission = this.authPermissionService.getCurrentPermission(Keys.PermissionsModule.Permission);
		this.Groups = this.authPermissionService.getCurrentPermission(Keys.PermissionsModule.Groups);
		this.isDisabled = this.Permission.Edit ? false : true;
		this.getRole();
		this.getApplications();
	}

	ngOnDestroy() { }

	constructor(
		private router: Router,
		private route: ActivatedRoute,
		private accountService: AccountService,
		private permissionService: PermissionService,
		private notify: NotificationsService,
		private storeService: StoresService,
		private authPermissionService: AuthPermissionService) { }


	getRole(): void {
		if (this.route.snapshot.params.id) {
			this.permissionService.get(this.route.snapshot.params.id).subscribe((response) => {
				this.role = response;

				this.role.permissions.forEach(item => {
					let array_resource_operations = item.resource.operations.replace(/ /g, "").split(",");
					let op;
					op = {
						approval: array_resource_operations.includes("Approval"),
						edit: array_resource_operations.includes("Edit"),
						remove: array_resource_operations.includes("Remove"),
						view: array_resource_operations.includes("View"),
						insert: array_resource_operations.includes("Insert")
					};
					item.resource.operations = op;
				});

				if (this.role.store != null) {
					this.selectStore.items = [{ id: this.role.store.code, text: this.role.store.name }];
					this.selectStore.active = [this.selectStore.itemObjects.find(x => x.id == this.role.store.code)];
					this.setStore(this.selectStore.active[0]);
				}

				if (this.role.account_roles.length > 0)
					this.viewAccount = true;

				if (this.role.permissions.length > 0) {
					this.viewResource = true;
				}
			});
		} else {
			this.role = new Role();
		}
		this.is_loaded = true;
	}

	public getPage(page: number) {
		this.roleFilter.skip = page - 1;

		this.searchResources();
	}

	searchAccount(): void {
		this.accountService.search(this.roleFilter).subscribe((response) => {
			this.accounts_list = response.items;
		});
	}

	searchResources(): void {
		let _app;
		let _resource;
		let _permission;
		let permission = new Array<Permission>();

		this.permissionService.getResources(this.roleFilter).subscribe((response) => {

			response.items.forEach(item => {
				let array_operations = item.operations.replace(/ /g, "").split(",");

				item.operations = {
					approval: array_operations.includes("Approval"),
					edit: array_operations.includes("Edit"),
					remove: array_operations.includes("Remove"),
					view: array_operations.includes("View"),
					insert: array_operations.includes("Insert")
				};
				_app = new Application(item.application.code, item.application.name);
				_resource = new Resource(item.code, item.name, item.operations, _app);
				_permission = new Permission("", "", _resource);

				permission.push(_permission);
			});

			this.Resources_list = permission;
			this.total_result_resource = response.total_result;
		});
	}

	insertAccount(value: any) {
		let result = this.role.account_roles.filter(a => a.account.code == value.code);

		if (result.length == 0) {
			let account = new Account(value.code, value.email, value.document);
			let accountRole = new AccountRole("", account);

			this.role.account_roles.push(accountRole);
			this.notify.create('Sucesso!', 'usuário inserido com sucesso', 'success', this.alert);
		}
		else {
			this.notify.create('Atenção!', 'usuário já inserido', 'warn', this.alert);
		}

		this.viewAccount = true;
	}

	insertResource(value: any) {
		let result = this.role.permissions.filter(x => x.resource.code == value.resource.code);

		if (result.length == 0) {
			this.role.permissions.push(value);
			this.notify.create('Sucesso!', 'recurso inserido com sucesso', 'success', this.alert);
		}
		else {
			this.notify.create('Atenção!', 'recurso já inserido', 'warn', this.alert);
		}

		this.viewResource = true;
	}

	operationsPermission(item: any, operation: string) {
		return item.operations.includes(operation);
	}

	changeOperations(item: any, operation: string, env: any) {
		let items = [];

		if (item.operations.length > 0)
			items = item.operations.replace(/ /g, "").split(",");

		if (env.target.checked) {
			items.push(operation);
		}
		else {
			if (items.indexOf(operation) >= 0)
				items.splice(items.indexOf(operation), 1);
		}

		item.operations = items.join(",");
	}

	removeAccount(value: any): void {
		this.role.account_roles.splice(this.role.account_roles.indexOf(value), 1);

		if (this.role.account_roles.length == 0)
			this.viewAccount = false
	}

	removePermission(value: any): void {
		this.role.permissions.splice(this.role.permissions.indexOf(value), 1);

		if (this.role.permissions.length == 0)
			this.viewResource = false;
	}

	save() {
		let op = this.role.permissions.find(x => x.operations == "");

		if (op) {
			this.notify.create('Atenção!', 'É necessário informar pelo menos <br> 1 (uma) operação para cada recurso.', 'warn', this.alert);
		} else {
			this.disableButton = true;
			let roleSave: any = {};

			if (this.role.code !== "")
				roleSave.code = this.role.code;

			roleSave.name = this.role.name;
			roleSave.description = this.role.description;
			roleSave.status = true;

			roleSave.store = {
				code: this.role.store.code
			};

			roleSave.permissions = this.role.permissions.map(x => {
				return {
					operations: x.operations,
					status: true,
					resource: {
						code: x.resource.code
					}
				};
			});

			roleSave.account_roles = this.role.account_roles.map(x => {
				return {
					account_code: x.account.code,
					status: true
				};
			});

			this.permissionService.saveRole(roleSave).subscribe((response) => {
				if (response) {
					this.notify.success('Sucesso!', 'Grupo criado com sucesso.');
					this.router.navigate(['/permissions/groups']);
				} else {
					this.disableButton = false;
				}
			});
		}
		this.disableButton = false;
	}

	getStores(term: any): void {
		if (term && term.length == 2) {
			this.storeService.getByTerm(term).subscribe((response) => {
				this.selectStore.items = response.map(result => {
					return { id: result.code, text: result.name }
				});
			});
		}
	}

	setStore(value: any): void {
		this.role.store.code = value.id;
		this.role.store.name = value.text;

		if (value.length > 0 || value.id) {
			this.disabledSave = false;
		} else {
			this.disabledSave = true;
		}
	}

	getApplications(): void {
		this.storeService.getApplications().subscribe((response) => {
			let members = response.filter(x => x.member.toLowerCase() === "merchant");
			this.selectApplication.items = members.map(result => {
				return { id: result.code, text: result.name }
			});
		});
	}

	addApplication(value: any): void {
		this.disableApplication = value.length !== 0 ? false : true;
		this.roleFilter.application_code = value.id;
		this.roleFilter.application_name = value.text;
	}

	isValid(role: Role) {
		if (role.permissions.length == 0)
			return false;
		return true;
	}

	openModalAccount() {
		this.showModalAccount = true;
	}
	closeModalAccount() {
		this.showModalAccount = false;
		this.accounts_list = [];
		this.roleFilter = new Filter();
	}
	openModalResource() {
		this.showModalResource = true;
	}
	closeModalResource() {
		this.showModalResource = false;
		this.Resources_list = [];
		this.selectApplication.active = [];
		this.roleFilter = new Filter();
	}
}
