import { Component, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '../accounts.service';
import { ActivatedRoute, Router } from '@angular/router';
import { StoresService } from '../../stores/stores.service';
import { PermissionService } from '../../permissions/permissions.service';
import { validateConfig } from '@angular/router/src/config';
import { SelectComponent, SelectItem } from 'ng2-select';
import { ConfirmationService } from '@jaspero/ng2-confirmations';
import { ResolveEmit } from '@jaspero/ng2-confirmations/src/interfaces/resolve-emit';
import { NotificationsService } from 'angular2-notifications';
import { BsModalComponent } from 'ng2-bs3-modal';
import { DatePipe } from '../../../../node_modules/@angular/common';
import { AuthPermissionService } from '../../shared/auth/auth-permission.service';
import { Keys } from '../../shared/models/keys';
import { OrderPipe } from 'ngx-order-pipe';

@Component({
	selector: 'app-account-detail',
	templateUrl: './account-detail.component.html',
	styleUrls: ['./account-detail.component.css']
})
export class AccountDetailComponent implements OnInit {

	@ViewChild('selectStore') selectStore: SelectComponent;
	@ViewChild('selectStoreBlocked') selectStoreBlocked: SelectComponent;
	@ViewChild('selectApplication') selectApplication: SelectComponent;
	@ViewChild('viewUnlock') modal: BsModalComponent;
	@ViewChild('viewRegistry') modalRegistry: BsModalComponent;
	@ViewChild('viewModalSms') modalSms: BsModalComponent;
	@ViewChild('selectStoreChangeEmail') selectStoreChangeEmail: SelectComponent;

	order: string = 'store.name';
	reverse: boolean = false;
	searchText = "";
	account: any = {};
	store: any = {};
	applications: any[] = [];
	applications_store: any[] = [];
	is_loaded: boolean;
	block: any = {};
	unlock: any = {};
	edit: any = {};
	registry: any = {};
	sms: any[] = [];
	changeAndSendEmail: any = {};
	filter: any = {
		skip: 0,
		take: 10
	};
	roles: any[] = [];

	currentUserPermission: any = {};
	currentAppPermission: any = {};
	currentUserBlockedPermission: any = {};

	constructor(
		private router: Router,
		private accountService: AccountService,
		private permissionService: PermissionService,
		private storeService: StoresService,
		private route: ActivatedRoute,
		private notifyConfirm: ConfirmationService,
        private notify: NotificationsService,
        private orderPipe: OrderPipe,
		private authPermissionService: AuthPermissionService) { }

	public ngOnInit() {

		this.currentUserPermission = this.authPermissionService.getCurrentPermission(Keys.UserModule.Account);
		this.currentAppPermission = this.authPermissionService.getCurrentPermission(Keys.UserModule.App);
		this.currentUserBlockedPermission = this.authPermissionService.getCurrentPermission(Keys.UserModule.Blocked);

		this.selectApplication.items = []
		this.selectStore.items = []
		this.selectStoreBlocked.items = [];
		this.selectStoreChangeEmail.items = [];
		this.get();
	}

	public unLinkRole(code: string): void {
		this.notifyConfirm.create('Atenção!', 'Deseja realmente remover esse usuário do grupo?').subscribe((ans: ResolveEmit) => {
			if (ans.resolved) {
				this.permissionService.unlinkAccount(this.account.code, code).subscribe(() => {
					this.notify.success('Sucesso!', 'Usuário removido do grupo com sucesso');
					this.router.navigate(['accounts']);
				});
			}
		});
	}

	setOrder(value: string) {
		if (this.order === value) {
		  this.reverse = !this.reverse;
		}
		this.order = value;
	}

	public get(): void {
		this.accountService.get(this.route.snapshot.params.id).subscribe((response) => {

			this.account = response;
			this.applications_store = response.applications_store;
			this.roles = this.orderPipe.transform(response.roles, 'store.name');
			this.is_loaded = true;

			if (this.applications_store)
				this.getApplications();
			else
				this.applications_store = [];
		});
	}

	public getApplications(): void {
		this.storeService.getApplications().subscribe((response) => {
			this.selectApplication.items = response.map(result => {
				return { id: result.code, text: result.name }
			});
		});
	}

	public getStores(term: any): void {
		if (term && term.length == 2) {
			this.storeService.getByTerm(term).subscribe((response) => {
				this.selectStore.items = response.map(result => {
					return { id: result.code, text: result.name }
				});
			});
		}
	}

	public setStore(value: any): void {
		this.store = {
			code: value.id,
			name: value.text
		};
	}

	public getStoresChangeEmail(term: any): void {
		if (term && term.length == 2) {
			this.storeService.getByTerm(term).subscribe((response) => {
				this.selectStoreChangeEmail.items = response.map(result => {
					return { id: result.code, text: result.name }
				});
			});
		}
	}

	public setStoreChangeEmail(value: any): void {
		this.changeAndSendEmail.store_code = value.id;
		this.changeAndSendEmail.store_name = value.text;
		this.filter.storePermission = value.id;
	}

	public clearStoreChangeEmail() {
		this.changeAndSendEmail = {};
		this.selectStoreChangeEmail.active = [];
	}

	public getStoresBlocked(term: any): void {
		if (this.selectStoreBlocked.options.length == 0) {
			let find = [{ code: '0', name: 'Todas as Lojas' }];

			this.selectStoreBlocked.items = find.map(result => {
				return { id: result.code, text: result.name }
			});
		}

		if (term && term.length == 2) {
			this.storeService.getByTerm(term).subscribe((response) => {
				this.selectStoreBlocked.items = response.map(result => {
					return { id: result.code, text: result.name }
				});
			});
		}
	}

	public setStoreBlocked(value: any): void {
		this.block.store_code = value.id;
		this.block.store_name = value.text;
	}

	public addApplications(value: any): void {
		this.applications = value.map(val => { return val.text });
	}

	public filterApplication(): void {
		if (this.applications.length == 0 && !this.store.code) {
			this.applications_store = this.account.applications_store;
		}
		else {
			this.applications_store = this.account.applications_store.filter(app => {

				var has_any_app = this.applications.find(app_name => app_name == app.name);

				if (this.store.code && this.applications.length > 0) {
					return this.store.code === app.store_code && has_any_app;
				}
				else if (this.store.code) {
					return this.store.code === app.store_code;
				}
				else {
					return has_any_app;
				}
			});
		}
	}

	public changeEmail(): void {
		let account_input = {
			code: this.account.code,
			email: this.account.email,
			email_new: this.account.email_new,
			store_code: this.changeAndSendEmail.store_code
		};

		this.accountService.changeAccountInfo(account_input).subscribe(() => {

			this.account.email = this.account.email_new;
			this.account.email_new = '';

			this.clearStoreChangeEmail();

			this.notify.success('Sucesso!', 'E-mail alterado com sucesso');
		});
	}

	public blockAccount(): void {
		if (this.block.reason) {
			this.notifyConfirm.create('Atenção!', 'Deseja realmente bloquear essa conta?').subscribe((ans: ResolveEmit) => {
				if (ans.resolved) {
					this.block.account_code = this.account.code;

					this.accountService.block(this.block).subscribe((response) => {
						this.notify.success('Sucesso!', 'Conta bloqueada com sucesso!');
						response.is_new = true;
						setTimeout(() => { response.is_new = false; }, 3000)
						this.account.blacklist = this.account.blacklist.filter(x => x.store_code != response.store_code);
						this.account.blacklist.unshift(response);

						//clear
						this.selectStoreBlocked.active = [];
						this.block = {};
					});
				}
			});
		}
	}

	public unlockAccount(): void {
		this.accountService.unlock(this.unlock).subscribe((response) => {
			this.notify.success('Sucesso!', 'Conta desbloquear com sucesso!');
			this.account.blacklist = this.account.blacklist.filter(x => x.store_code != response.store_code);

			//clear
			this.unlock = {};

			//Close model of unlock account
			this.modal.close();
		});
	}

	public delete(): void {
		this.notifyConfirm.create('Atenção!', 'Deseja realmente remover essa conta?').subscribe((ans: ResolveEmit) => {
			if (ans.resolved) {
				this.accountService.delete(this.account.code).subscribe(() => {
					this.notify.success('Sucesso!', 'Conta removida com sucesso');
					this.router.navigate(['accounts']);
				});
			}
		});
	}

	public sendPasswordEmail(): void {
		this.notifyConfirm.create('Atenção!', 'Deseja enviar o e-mail de esqueci minha senha para o cliente?').subscribe((ans: ResolveEmit) => {
			if (ans.resolved) {
				this.accountService.sendPasswordEmail(this.account.email).subscribe(() => {
					this.notify.success('Sucesso!', 'E-mail enviado com sucesso');
				});
			}
		});
	}

	public unlockUser() {
		this.notifyConfirm.create('Atenção!', 'Deseja realmente desbloquear esse usuário?').subscribe((ans: ResolveEmit) => {
			if (ans.resolved) {
				this.accountService.unlockUser(this.account.code).subscribe((response) => {
					this.notify.success('Sucesso!', 'Usuário desbloqueado com sucesso');
					this.account = response;
				});
			}
		});
	}

	public openUnlockModal(storeCode: any) {
		this.unlock = {};
		this.unlock.store_code = storeCode;
		this.unlock.account_code = this.account.code;

		//Open model of unlock account
		this.modal.open();
	}

	public editDocument(): void {
		this.edit.document = true;
		this.edit.mask = (this.account.document.length > 11) ? "00.000.000/0000-00" : "000.000.000-00";
		this.edit.document_new = this.account.document;
	}

	public cancelEditDocument(): void {
		this.edit.document = false;
		this.edit.document_new = '';
	}

	public saveDocument(): void {
		this.notifyConfirm.create('Atenção!', 'Deseja alterar o documento desse usuário?').subscribe((ans: ResolveEmit) => {
			if (ans.resolved) {
				let account_input = {
					code: this.account.code,
					document: this.account.document,
					document_new: this.edit.document_new
				};

				this.accountService.changeAccountInfo(account_input).subscribe(() => {

					this.account.document = this.edit.document_new;
					this.edit.document_new = '';

					this.notify.success('Sucesso!', 'Documento alterado com sucesso');
				});
			}
		});

		this.edit.document = false;
	}

	public editBirthdate(): void {
		this.edit.birth_date = true;
		this.edit.birth_date_new = this.account.customer.birth_date;
	}

	public saveBirthdate(): void {
		this.notifyConfirm.create('Atenção!', 'Deseja alterar a data de nascimento desse usuário?').subscribe((ans: ResolveEmit) => {
			if (ans.resolved) {
				let account_input = {
					code: this.account.code,
					birth_date_new: this.edit.birth_date_new
				};
				console.log(account_input);

				this.accountService.changeAccountInfo(account_input).subscribe(() => {

					this.account.customer.birth_date = this.edit.birth_date_new;
					this.edit.birth_date_new = '';

					this.notify.success('Sucesso!', 'Data de nascimento alterada com sucesso');
				});
			}
		});

		this.edit.birth_date = false;
	}

	public cancelEditBirthdate(): void {
		this.edit.birth_date = false;
		this.edit.birth_date_new = '';
	}

	public openRegistry(): void {
		let date = "";

		if (this.account.document.length == 11)
			date = new DatePipe("en-US").transform(this.account.customer.birth_date, "yyyy-MM-dd");

		this.accountService.getRegistry(this.account.document, date).subscribe((result) => {
			this.registry = result;
			this.modalRegistry.open();
		});
	}

	public closeRegistry(): void {
		this.modalRegistry.close();
	}

	public openModalSms(): void {
		this.sms = [];
		this.accountService.getSms(this.account.code).subscribe((result) => {
			this.sms = result;
			this.modalSms.open("lg");
		});
	}

	public closeModalSms(): void {
		this.modalSms.close();
	}
}
