import { Component, OnInit, AfterContentInit, AfterContentChecked } from '@angular/core';
import { AuthPermissionService } from '../../shared/auth/auth-permission.service';
import { Keys } from '../../shared/models/keys';

@Component({
	selector: 'app-master-menu',
	templateUrl: './master-menu.component.html'
})
export class MasterMenuComponent implements OnInit, AfterContentChecked {
	isLoad: boolean = false;
	currentPermission: any = {
		User: null,
		Metadados: null,
		Stores: null,
		Imports: null,
		Modules: null,
		Groups: null
	};

	constructor(
		private authPermissionService: AuthPermissionService) {
	}

	ngOnInit() {}

	ngAfterContentChecked(){
		if (this.currentPermission.User == null){
			this.currentPermission.User = this.authPermissionService.getCurrentPermission(Keys.UserModule.Account);
			this.currentPermission.Imports = this.authPermissionService.getCurrentPermission(Keys.UserModule.Imports);
			this.currentPermission.Stores = this.authPermissionService.getCurrentPermission(Keys.StoresModule.Stores);
			this.currentPermission.Metadados = this.authPermissionService.getCurrentPermission(Keys.MetadadosModule.Metadados);
			this.currentPermission.Modules = this.authPermissionService.getCurrentPermission(Keys.PermissionsModule.Modules);
			this.currentPermission.Groups = this.authPermissionService.getCurrentPermission(Keys.PermissionsModule.Groups);
		}
		else
		{
			this.isLoad = true;
		}
	}
}
