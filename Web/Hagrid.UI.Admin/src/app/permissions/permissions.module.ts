import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PermissionsRoutingModule } from './permissions.routing';
import { PermissionsComponent } from './permissions.component';
import { PermissionService } from './permissions.service';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'ng2-select';
import { PipesModule } from '../shared/pipes/pipes.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { BsModalModule } from 'ng2-bs3-modal';
import { NgxMaskModule } from 'ngx-mask';
import { Ng4LoadingSpinnerModule } from 'ng4-loading-spinner';
import { PermissionResourceListComponent } from './permission-resource-list/permission-resource-list.component';
import { PermissionRolesComponent } from './permission-roles/permission-roles.component';
import { PermissionRoleCreateComponent } from './permission-roles/permission-role-create/permission-role-create.component';
import { StoreSelectComponent } from '../shared/components/store-select/store-select.component';
import { ApplicationSelectComponent } from '../shared/components/application-select/application-select.component';

@NgModule({
	imports: [
		FormsModule,
		CommonModule,
		SelectModule,
		PermissionsRoutingModule,
		PipesModule,
		NgxPaginationModule,
		NgxMaskModule.forRoot(),
		BsModalModule,
		Ng4LoadingSpinnerModule.forRoot()
	],
	declarations: [
		PermissionsComponent,
		PermissionResourceListComponent,
		PermissionRolesComponent,
		PermissionRoleCreateComponent,
		StoreSelectComponent,
		ApplicationSelectComponent
	],
	providers: [PermissionService]
})
export class PermissionsModule { }
