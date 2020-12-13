import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MetadataComponent } from './metadata.component';
import { FormsModule } from '@angular/forms';
import { NgJsonEditorModule } from '../../shared/jsoneditor/ng-jsoneditor.module';
import { NgxMaskModule } from 'ngx-mask';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		NgJsonEditorModule,
		NgxMaskModule.forRoot()
	],
	declarations: [
		MetadataComponent
	],
	exports: [
		MetadataComponent
	]
})
export class MetadataComponentModule { }
