import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StoreDetailComponent } from './store-detail/store-detail.component';
import { StoreCreateComponent } from './store-create/store-create.component';
import { StoreListComponent } from './store-list/store-list.component';
import { StoresComponent } from './stores.component';
import { AuthPermissionService } from '../shared/auth/auth-permission.service';
import { Keys } from '../shared/models/keys';

const routes: Routes = [
  {
      path: '', component: StoresComponent,
      children: [
          { path: '', component: StoreListComponent, canActivate: [AuthPermissionService], data: { role: Keys.StoresModule.Stores }  },
          { path: 'create', component: StoreCreateComponent, canActivate: [AuthPermissionService], data: { role: Keys.StoresModule.Stores, type: 'insert' } },
          { path: ':id', component: StoreDetailComponent, canActivate: [AuthPermissionService], data: { role: Keys.StoresModule.Stores }  }
      ]
  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StoresRoutingModule { }
