import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountsRoutingModule } from './accounts.routing';
import { AccountsComponent } from './accounts.component';
import { AccountListComponent } from './account-list/account-list.component';
import { AccountDetailComponent } from './account-detail/account-detail.component';
import { AccountService } from './accounts.service';
import { FormsModule } from '@angular/forms';
import { StoresService } from '../stores/stores.service';
import { SelectModule } from 'ng2-select';
import { PipesModule } from '../shared/pipes/pipes.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { AccountImportComponent } from './account-import/account-import.component';
import { BsModalModule } from 'ng2-bs3-modal';
import { MetadataComponentModule } from '../metadatas/metadata/metadata.component.module';
import { AccountCreateComponent } from './account-create/account-create.component';
import { NgxMaskModule } from 'ngx-mask';
import { OrderModule } from 'ngx-order-pipe';

@NgModule({
	imports: [
		FormsModule,
		CommonModule,
		SelectModule,
		AccountsRoutingModule,
		PipesModule,
		NgxPaginationModule,
		NgxMaskModule.forRoot(),
		BsModalModule,
		MetadataComponentModule,
		OrderModule
	],
	declarations: [
		AccountsComponent,
		AccountListComponent,
		AccountDetailComponent,
		AccountImportComponent,
		AccountCreateComponent
	],
	providers: [AccountService, StoresService]
})
export class AccountsModule { }
