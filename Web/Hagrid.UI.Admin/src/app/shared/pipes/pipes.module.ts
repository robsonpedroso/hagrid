import { ZipcodePipe } from './zipcode.pipe';
import { DocumentPipe } from './document.pipe';
import { PhonePipe } from './phone.pipe';
import { NgModule } from '@angular/core';
import { OrderByPipe } from './order-by.pipe';
import { AccountSearchPipe } from './account-search.pipe';
import { PermissionSearchPipe } from './permission-search.pipe';
import { GroupPermissionPipe } from './group-permission.pipe';

@NgModule({
    declarations: [
        ZipcodePipe,
        DocumentPipe,
        PhonePipe,
		OrderByPipe,
		AccountSearchPipe,
		PermissionSearchPipe,
		GroupPermissionPipe
    ],
    exports: [
        ZipcodePipe,
        DocumentPipe,
        PhonePipe,
		OrderByPipe,
		AccountSearchPipe,
		PermissionSearchPipe,
		GroupPermissionPipe
    ]
})
export class PipesModule { }
