import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MetadatasRoutingModule } from './metadatas.routing';
import { MetadatasComponent } from './metadatas.component';
import { MetadataFieldComponent } from './metadata-field/metadata-field.component';
import { MetadatasService } from './metadatas.service';
import { FormsModule } from '@angular/forms';
import { StoresService } from '../stores/stores.service';
import { SelectModule } from 'ng2-select';
import { PipesModule } from '../shared/pipes/pipes.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgJsonEditorModule } from '../shared/jsoneditor/ng-jsoneditor.module';
import { TagInputModule } from 'ngx-chips';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // this is needed!

@NgModule({
	imports: [
		FormsModule,
		CommonModule,
		SelectModule,
		MetadatasRoutingModule,
		PipesModule,
		NgxPaginationModule,
		NgJsonEditorModule,
		TagInputModule
	],
	declarations: [
		MetadatasComponent,
		MetadataFieldComponent
	],
	providers: [MetadatasService]
})
export class MetadatasModule {}
