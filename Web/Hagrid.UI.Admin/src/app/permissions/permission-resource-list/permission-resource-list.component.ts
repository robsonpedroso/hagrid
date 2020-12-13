import { Component, OnInit, ViewChild } from '@angular/core';
import { PermissionService } from '../permissions.service';
import { StoresService } from '../../stores/stores.service';
import { SelectComponent } from 'ng2-select';
import { ConfirmationService } from '@jaspero/ng2-confirmations';
import { ResolveEmit } from '@jaspero/ng2-confirmations/src/interfaces/resolve-emit';
import { NotificationsService } from 'angular2-notifications';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AuthPermissionService } from '../../shared/auth/auth-permission.service';
import { Keys } from '../../shared/models/keys';
import { Router } from '@angular/router';

@Component({
	selector: 'app-permission-resource-list',
	templateUrl: './permission-resource-list.component.html',
	styles: []
})
export class PermissionResourceListComponent implements OnInit {

	@ViewChild('selectApplication') selectApplication: SelectComponent;
	@ViewChild('selectedApplicationSave') selectedApplicationSave: SelectComponent;

	resourceSave = {
		internal_code: "",
		name: "",
		description: "",
		application: {
			code: "",
			name: ""
		},
		flag_operations: {
			view: false,
			insert: false,
			edit: false,
			remove: false,
			approval: false
		},
		operations: {}
	};
	is_loaded: boolean;
	filter: any = {
		skip: 0,
		take: 10,
		name: ''
	};
	resources: any = {};
	show_modal: boolean;
	disableApplication: boolean = true;
	currentModulePermission: any = {};
	alert: any = {
		timeOut: 3000,
		showProgressBar: false,
		pauseOnHover: true,
		clickToClose: true,
		animate: 'fromLeft'
	};

	constructor(
		private permissionService: PermissionService,
		private storeService: StoresService,
		private notify: NotificationsService,
		private notifyConfirm: ConfirmationService,
		private spinnerService: Ng4LoadingSpinnerService,
		private authPermissionService: AuthPermissionService,
		private router: Router) { }

	ngOnInit() {
		this.getApplications();
		this.currentModulePermission = this.authPermissionService.getCurrentPermission(Keys.PermissionsModule.Modules);
	}

	public isValid(resource: any) {

		if (!resource.flag_operations.approval &&
			!resource.flag_operations.view &&
			!resource.flag_operations.insert &&
			!resource.flag_operations.remove &&
			!resource.flag_operations.edit)
			return false;

		return true;
	}

	public getPage(page: number) {
		this.filter.skip = page - 1;
		this.search();
	}

	public search(): void {
		this.permissionService.getResources(this.filter).subscribe((response) => {

			this.resources = response;

			this.resources.items.forEach(element => {
				let array_operations = element.operations.replace(/ /g, "").split(",");

				element.flag_operations = {
					approval: array_operations.includes("Approval"),
					edit: array_operations.includes("Edit"),
					remove: array_operations.includes("Remove"),
					view: array_operations.includes("View"),
					insert: array_operations.includes("Insert")
				};
			})
		});
	}

	public openAddModal() {
		this.resourceSave = {
			internal_code: "",
			name: "",
			description: "",
			application: {
				code: "",
				name: ""
			},
			flag_operations: {
				view: false,
				insert: false,
				edit: false,
				remove: false,
				approval: false
			},
			operations: ""
		};
		this.show_modal = true;
	}

	public edit(obj: any): void {
		if (!this.currentModulePermission.Insert)
		{
			this.show_modal = false;
			this.notify.create('Atenção!', 'Usuário sem permissão para executar essa ação', 'warn', this.alert);
			this.router.navigate(['/home']);
		}
		this.resourceSave = JSON.parse(JSON.stringify(obj));
		this.selectedApplicationSave.active = [this.selectedApplicationSave.itemObjects.find(x => x.id == this.resourceSave.application.code)];
		this.show_modal = true;
	}

	public convertOperations(obj: any) {
		let opr = [];
		if (obj.flag_operations.approval)
			opr.push('Approval');
		if (obj.flag_operations.edit)
			opr.push('Edit');
		if (obj.flag_operations.insert)
			opr.push('Insert');
		if (obj.flag_operations.view)
			opr.push('View');
		if (obj.flag_operations.remove)
			opr.push('Remove');

		this.resourceSave.operations = opr.join(', ');
	}

	public save(objSave: any): void {
		this.spinnerService.show();

		this.convertOperations(objSave);

		this.permissionService.saveResources(this.resourceSave).subscribe((response) => {
			if (response.code) {
				this.notify.success('Sucesso!', 'Módulo criado com sucesso.');
				this.spinnerService.hide();
				this.closeModal();
				this.search();
			}
		});
	}

	public remove(code: string): void {
		this.notifyConfirm.create('Atenção!', 'Deseja realmente remover esse módulo?').subscribe((ans: ResolveEmit) => {
			if (ans.resolved) {
				this.permissionService.removeResources(code).subscribe((response) => {
					if (response.status = 'OK') {
						this.notify.success('Sucesso!', 'Módulo removido com sucesso');
						this.search();
					}
				});
			}
		});
	}

	public closeModal(): void {
		this.resourceSave = {
			internal_code: "",
			name: "",
			description: "",
			application: {
				code: "",
				name: ""
			},
			flag_operations: {
				view: false,
				insert: false,
				edit: false,
				remove: false,
				approval: false
			},
			operations: ""
		};
		this.selectedApplicationSave.active = [];
		this.show_modal = false;
	}

	public cancel(): void {
		this.closeModal();
	}

	public getApplications(): void {
		this.storeService.getApplications().subscribe((response) => {
			let members = response.filter(x => x.member.toLowerCase() === "merchant");
			this.selectApplication.items = this.selectedApplicationSave.items = members.map(result => {
				return { id: result.code, text: result.name }
			});
		});
	}

	public addApplication(value: any): void {
		this.disableApplication = value.length !== 0 ? false : true;
		this.filter.application_code = value.id;
	}

	public editSelected(value: any): void {
		this.resourceSave.application.code = value.id;
		this.resourceSave.application.name = value.text;
	}
}
