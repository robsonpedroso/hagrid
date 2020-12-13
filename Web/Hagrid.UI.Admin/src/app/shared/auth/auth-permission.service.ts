import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { SessionStorageService } from 'ngx-webstorage';
import { Keys } from '../models/keys';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { NotificationsService } from 'angular2-notifications';


@Injectable()
export class AuthPermissionService implements CanActivate {
	roles: any[];
	alert: any = {
		timeOut: 3000,
		showProgressBar: false,
		pauseOnHover: true,
		clickToClose: true,
		animate: 'fromLeft'
	};

	constructor(
		private sessionStorageService: SessionStorageService,
		private router: Router,
		private notify: NotificationsService
	) { }

	canActivate(route: ActivatedRouteSnapshot) {
		let permissions = this.getCurrentPermission(route.data.role);

		if (route.data.type == 'insert') {
			if (permissions.Insert)
			{
				return true;
			}
			this.messageReturn();
			return false;
		}
		if (permissions.View) {
			return true;
		}

		this.messageReturn();
		return false;
	}

	messageReturn() {
		this.notify.create('Atenção!', 'Usuário sem permissão para executar essa ação', 'warn', this.alert);
		this.router.navigate(['/home']);
	}

	getCurrentPermission(internalCode: string): any {
		this.roles = this.sessionStorageService.retrieve(Keys.StoragesRole);
		let currentPermission: any = null;
		if (this.roles) {

			currentPermission = {
				Approval: false,
				Edit: false,
				Remove: false,
				View: false,
				Insert: false
			};

			this.roles.forEach(r => {
				r.permissions.forEach(p => {
					if (typeof(p.resource.internal_code) != 'undefined' && p.resource.internal_code.toLowerCase() == internalCode.toLowerCase()) {
						let array_operations = p.operations.replace(/ /g, "").split(",");

						if (!currentPermission.Approval)
							currentPermission.Approval = array_operations.includes("Approval");

						if (!currentPermission.Edit)
							currentPermission.Edit = array_operations.includes("Edit");

						if (!currentPermission.Remove)
							currentPermission.Remove = array_operations.includes("Remove");

						if (!currentPermission.View)
							currentPermission.View = array_operations.includes("View");

						if (!currentPermission.Insert)
							currentPermission.Insert = array_operations.includes("Insert");
					}
				});
			});
		}
		return currentPermission;
	}
}
