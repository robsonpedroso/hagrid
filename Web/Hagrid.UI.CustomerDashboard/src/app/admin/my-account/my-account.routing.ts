import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MyAccountComponent } from './my-account.component';
import { MyAccountHomeComponent } from './my-account-home/my-account-home.component';
import { MyAccountProfileEditComponent } from './my-account-profile/my-account-profile-edit/my-account-profile-edit.component';
import { MyAccountAdressAddComponent } from './my-account-address/my-account-adress-add/my-account-adress-add.component';
import { MyAccountAddressAdmComponent } from './my-account-address/my-account-address-adm/my-account-address-adm.component';
import { MyAccountMailAdmComponent } from './my-account-mail/my-account-mail-adm/my-account-mail-adm.component';
import { MyAccountPhoneAdmComponent } from './my-account-phone/my-account-phone-adm/my-account-phone-adm.component';
import { MyAccountPasswordAdmComponent } from './my-account-password/my-account-password-adm/my-account-password-adm.component';

const routes: Routes = [
  {path: '', component: MyAccountHomeComponent},
  { path: 'profile-edit/:id', component: MyAccountProfileEditComponent },

  { path: 'address-add', component: MyAccountAdressAddComponent },
  { path: 'address-add/:id', component: MyAccountAdressAddComponent },
  { path: 'address-adm', component: MyAccountAddressAdmComponent },

  { path: 'mail-adm', component: MyAccountMailAdmComponent },

  { path: 'phone-adm', component: MyAccountPhoneAdmComponent },

  { path: 'password-adm', component: MyAccountPasswordAdmComponent },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyAccountRoutingModule { }

