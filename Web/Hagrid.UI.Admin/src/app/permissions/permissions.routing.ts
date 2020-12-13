import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PermissionsComponent } from './permissions.component';
import { PermissionResourceListComponent } from './permission-resource-list/permission-resource-list.component';
import { PermissionRolesComponent } from './permission-roles/permission-roles.component';
import { PermissionRoleCreateComponent } from './permission-roles/permission-role-create/permission-role-create.component';
import { AuthPermissionService } from '../shared/auth/auth-permission.service';
import { Keys } from '../shared/models/keys';

const routes: Routes = [
	{
		path: '', component: PermissionsComponent,
		children: [
			{ path: 'resources', component: PermissionResourceListComponent, canActivate: [AuthPermissionService], data: { role: Keys.PermissionsModule.Modules }   },
			{ path: 'groups', component: PermissionRolesComponent, canActivate: [AuthPermissionService], data: { role: Keys.PermissionsModule.Groups }   },
			{ path: 'groups/create', component: PermissionRoleCreateComponent, canActivate: [AuthPermissionService], data: { role: Keys.PermissionsModule.Groups, type: 'insert' }   },
			{ path: 'groups/:id', component: PermissionRoleCreateComponent, canActivate: [AuthPermissionService], data: { role: Keys.PermissionsModule.Groups, type: 'insert' }   }
		]
	}];


@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})

export class PermissionsRoutingModule { }
