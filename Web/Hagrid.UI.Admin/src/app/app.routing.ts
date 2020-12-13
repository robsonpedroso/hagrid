import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from './shared/auth/auth-guard.service';
import { AppComponent } from './app.component';
import { Error404Component } from './errors/error-404/error-404.component';
import { Error403Component } from './errors/error-403/error-403.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full',
        canActivate: [AuthGuardService]
    },
    {
        path: 'home',
        loadChildren: './home/home.module#HomeModule',
        canActivate: [AuthGuardService]
    },
    {
        path: 'accounts',
        loadChildren: './accounts/accounts.module#AccountsModule',
        canActivate: [AuthGuardService]
	},
	{
        path: 'stores',
        loadChildren: './stores/stores.module#StoresModule',
        canActivate: [AuthGuardService]
	},
	{
        path: 'metadatas',
        loadChildren: './metadatas/metadatas.module#MetadatasModule',
        canActivate: [AuthGuardService]
	},
	{
		path: 'permissions',
		loadChildren: './permissions/permissions.module#PermissionsModule',
		canActivate: [AuthGuardService]
	},
    {
        path: 'authorized/:transfer_token',
        component: AppComponent,
        canActivate: [AuthGuardService],
        data: { type: 'authorized' }
    },
    {
        path: 'forbidden',
        component: Error403Component
    },
    {
        path: '**',
        component: Error404Component
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    declarations: [],
    exports: [RouterModule]
})
export class AppRoutingModule { }
