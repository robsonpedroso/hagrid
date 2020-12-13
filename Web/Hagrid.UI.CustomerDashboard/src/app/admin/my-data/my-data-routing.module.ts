import { MyDataComponent } from './my-data.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../../shared/auth/auth-guard.service';


const myDataRoutes: Routes = [
  { 
    path: '', component: MyDataComponent ,
    children: [
      {
        path: '',
        loadChildren: "../my-account/my-account.module#MyAccountModule"
      },
      {
        path: "conta",
        loadChildren: "../my-account/my-account.module#MyAccountModule",
        canActivate: [AuthGuardService]
      },
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(myDataRoutes)],
  exports: [RouterModule]
})

export class MyDataRoutingModule { }
