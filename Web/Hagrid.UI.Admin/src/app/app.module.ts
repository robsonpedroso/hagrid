import { LogisticsService } from './shared/services/logistics.service';

import { AppRoutingModule } from './app.routing';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, Injector } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgProgressModule } from '@ngx-progressbar/core';
import { NgProgressHttpModule } from '@ngx-progressbar/http';
import { SimpleNotificationsModule } from 'angular2-notifications';
import { JasperoConfirmationsModule } from '@jaspero/ng2-confirmations';
import { NotificationsService } from 'angular2-notifications';
import { Ng2Webstorage, SessionStorageService } from 'ngx-webstorage';

//Shared
import { AuthService } from './shared/auth/auth.service';
import { AuthGuardService } from './shared/auth/auth-guard.service';
import { AuthInterceptor } from './shared/interceptors/request-interceptor.service';

//Modulos RK
import { HomeModule } from './home/home.module';
import { AccountsModule } from './accounts/accounts.module';
import { StoresModule } from './stores/stores.module';
import { PermissionsModule } from './permissions/permissions.module';
import { MetadatasModule } from './metadatas/metadatas.module';

import { AppComponent } from './app.component';
import { MasterHeaderComponent } from './master/master-header/master-header.component';
import { MasterMenuComponent } from './master/master-menu/master-menu.component';
import { MasterFooterComponent } from './master/master-footer/master-footer.component';
import { Error404Component } from './errors/error-404/error-404.component';
import { Error403Component } from './errors/error-403/error-403.component';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { Keys } from './shared/models/keys';
import { AuthPermissionService } from './shared/auth/auth-permission.service';


@NgModule({
    declarations: [
        AppComponent,
        MasterHeaderComponent,
        MasterMenuComponent,
        MasterFooterComponent,
        Error404Component,
		Error403Component
	],
    imports: [
        AppRoutingModule,
        HomeModule,
        AccountsModule,
        BrowserModule,
        BrowserAnimationsModule,
        JasperoConfirmationsModule,
        SimpleNotificationsModule.forRoot(),
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        Ng2Webstorage.forRoot({
            prefix: 'Hagrid-Storage',
            separator: '-',
            caseSensitive: true
        }),
        NgProgressModule.forRoot({
            spinner: false,
            spinnerPosition: 'left',
            color: '#222d32',
            thick: true
        }),
        NgProgressHttpModule,
		StoresModule,
		PermissionsModule,
		MetadatasModule,
	],
    providers: [{
        provide: HTTP_INTERCEPTORS,
        useClass: AuthInterceptor,
        multi: true,
        deps: [Injector, Router, NotificationsService, SessionStorageService]
    }, AuthService, AuthGuardService,LogisticsService, AuthPermissionService],
	bootstrap: [AppComponent],
	exports: []
})
export class AppModule { }
