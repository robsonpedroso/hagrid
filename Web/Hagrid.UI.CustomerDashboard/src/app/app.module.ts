import { AppRoutingModule } from './app.routing';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, Injector, LOCALE_ID } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Router } from '@angular/router';
import { SimpleNotificationsModule } from 'angular2-notifications';
import { JasperoConfirmationsModule } from '@jaspero/ng2-confirmations';
import { NotificationsService } from 'angular2-notifications';
import { Ng2Webstorage, SessionStorageService } from 'ngx-webstorage';

// Shared
import { AuthService } from './shared/auth/auth.service';
import { AuthGuardService } from './shared/auth/auth-guard.service';
import { AuthInterceptor } from './shared/interceptors/request-interceptor.service';

// Modulos RK
import { DashboardModule } from './admin/dashboard/dashboard.module';
import { MyDataModule } from './admin/my-data/my-data.module';
import { MyCardsModule } from './admin/my-cards/my-cards.module';
import { SuperpointsModule } from './admin/superpoints/superpoints.module';

import { AppComponent } from './app.component';
import { MasterHeaderComponent } from './master/master-header/master-header.component';
import { MasterMenuComponent } from './master/master-menu/master-menu.component';
import { MasterUserComponent } from './master/master-user/master-user.component';
import { MasterFooterComponent } from './master/master-footer/master-footer.component';
import { Error404Component } from './errors/error-404/error-404.component';
import { Error403Component } from './errors/error-403/error-403.component';

import { registerLocaleData } from '@angular/common';
import ptBr from '@angular/common/locales/pt';
registerLocaleData(ptBr)

import {MatTabsModule, MatDatepickerModule, MatFormFieldModule, MatNativeDateModule, MatInputModule} from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';

@NgModule({
    imports: [
        AppRoutingModule,
        BrowserModule,
        BrowserAnimationsModule,
        JasperoConfirmationsModule,
        SimpleNotificationsModule.forRoot(),
        HttpClientModule,
        MyDataModule,
        MyCardsModule,
        DashboardModule,
        SuperpointsModule,
        MatTabsModule,
        MatDatepickerModule,
        MatFormFieldModule,
        MatNativeDateModule,
        MatMomentDateModule,
        MatInputModule,
        Ng2Webstorage.forRoot({
          prefix: 'CustomerDashboard-Storage',
          separator: '-',
          caseSensitive: true
        }),
    ],
    declarations: [
      AppComponent,
      MasterHeaderComponent,
      MasterMenuComponent,
      MasterUserComponent,
      MasterFooterComponent,
      Error404Component,
      Error403Component
  ],
    providers: [
      {
        provide: HTTP_INTERCEPTORS,
        useClass: AuthInterceptor,
        multi: true,
        deps: [Injector, Router, NotificationsService, SessionStorageService]
      },
      AuthService,
      AuthGuardService,
      { provide: LOCALE_ID, useValue: 'pt-BR' }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
