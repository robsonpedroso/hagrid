import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StoresRoutingModule } from './stores.routing';
import { StoresComponent } from './stores.component';
import { StoreDetailComponent } from './store-detail/store-detail.component';
import { StoreListComponent } from './store-list/store-list.component';
import { StoreCreateComponent } from './store-create/store-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

//Plugins
import { PipesModule } from '../shared/pipes/pipes.module';
import { NgxMaskModule } from 'ngx-mask';
import { ClipboardModule } from 'ngx-clipboard';
import { MetadataComponentModule } from '../metadatas/metadata/metadata.component.module';
import { ShowErrorsComponent } from "../shared/components/ShowErrors.component";
import { FieldErrorDisplayComponent } from '../shared/components/field-error-display.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxBrModule } from '@nbfontana/ngx-br';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    StoresRoutingModule,
    PipesModule,
	NgxMaskModule.forRoot(),
	NgxBrModule.forRoot(),
	ClipboardModule,
	MetadataComponentModule,
	NgxPaginationModule
	],
  declarations: [
    StoresComponent,
    StoreDetailComponent,
    StoreListComponent, StoreCreateComponent, ShowErrorsComponent, FieldErrorDisplayComponent
  ],
	exports: [PipesModule]
})
export class StoresModule { }
