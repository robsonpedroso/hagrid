import { Component, OnInit, ViewChild } from '@angular/core';
import { StoresService } from '../../stores/stores.service';
import { PermissionService } from '../permissions.service';
import { SelectComponent } from 'ng2-select';
import { ConfirmationService } from '@jaspero/ng2-confirmations';
import { ResolveEmit } from '@jaspero/ng2-confirmations/src/interfaces/resolve-emit';
import { NotificationsService } from 'angular2-notifications';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthPermissionService } from '../../shared/auth/auth-permission.service';
import { Keys } from '../../shared/models/keys';

@Component({
	selector: 'app-permission-roles',
	templateUrl: './permission-roles.component.html',
	styles: []
})
export class PermissionRolesComponent implements OnInit {

	@ViewChild('selectStore') selectStore: SelectComponent;

	store: any = {};
	roles: any = {};
	filter: any = {
		skip: 0,
		take: 10
	};
	disableStore: boolean = true;
	currentGroupSearchPermission: any = {};

	constructor(
		private router: Router,
		private storeService: StoresService,
		private permissionService: PermissionService,
		private notifyConfirm: ConfirmationService,
		private notify: NotificationsService,
		private authPermissionService: AuthPermissionService) { }

	ngOnInit() {
		this.currentGroupSearchPermission = this.authPermissionService.getCurrentPermission(Keys.PermissionsModule.Groups);
	}

	public getPage(page: number) {
		this.filter.skip = page - 1;
        this.search();
    }

	public search(): void {
		this.permissionService.getRoles(this.filter).subscribe((response) => {
			this.roles = response;
		});
	}

	public removeRole(code: string): void {
		this.notifyConfirm.create('Atenção!', 'Deseja realmente remover esse grupo?').subscribe((ans: ResolveEmit) => {
			if (ans.resolved) {
				this.permissionService.removeRole(code).subscribe(() => {
					this.notify.success('Sucesso!', 'Grupo removido com sucesso');
					this.router.navigate(['/permissions/groups']);
				});
			}
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
		this.disableStore = value.length !== 0 ? false : true;
		this.filter.storePermission = value.id;
	}

}
