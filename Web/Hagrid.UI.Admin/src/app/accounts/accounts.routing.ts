import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountsComponent } from './accounts.component';
import { AccountListComponent } from './account-list/account-list.component';
import { AccountDetailComponent } from './account-detail/account-detail.component';
import { AccountCreateComponent } from './account-create/account-create.component';
import { AccountImportComponent } from './account-import/account-import.component';
import { AuthPermissionService } from '../shared/auth/auth-permission.service';
import { Keys } from '../shared/models/keys';

const routes: Routes = [
    {
        path: '', component: AccountsComponent,
        children: [
            { path: '', component: AccountListComponent, canActivate: [AuthPermissionService], data: { role: Keys.UserModule.Account }  },
            { path: 'create', component: AccountCreateComponent, canActivate: [AuthPermissionService], data: { role: Keys.UserModule.Account, type: 'insert' } },
            { path: 'import', component: AccountImportComponent, canActivate: [AuthPermissionService], data: { role: Keys.UserModule.Imports }  },
            { path: ':id', component: AccountDetailComponent, canActivate: [AuthPermissionService], data: { role: Keys.UserModule.Account }  },
        ]
    }];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class AccountsRoutingModule { }
