import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';

import { CustomerService } from '../../shared/services/customer.service';
import { AuthService } from './../../shared/auth/auth.service';
import { SuperpointsService } from '../../shared/services/superpoints.service';
import { MyCardsModule } from '../my-cards/my-cards.module';
import { DashboardOrdersComponent } from './dashboard-orders/dashboard-orders.component';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';
import { OrdersService } from '../../shared/services/orders.service';
import { PipesModule } from '../../shared/pipes/pipes.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { SuperpointsModule } from '../superpoints/superpoints.module';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatTabsModule, MatDatepickerModule, MatFormFieldModule, MatInputModule} from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    DashboardRoutingModule,
    MyCardsModule,
    SuperpointsModule,
    PipesModule,
    NgxPaginationModule,
    HttpModule,
    FormsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatTabsModule,
    ReactiveFormsModule
  ],
  declarations: [
    DashboardComponent,
    DashboardOrdersComponent,
    DashboardHomeComponent
  ],
  providers: [CustomerService, AuthService, SuperpointsService, OrdersService]
})
export class DashboardModule { }

