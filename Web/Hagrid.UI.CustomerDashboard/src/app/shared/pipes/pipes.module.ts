import { ZipcodePipe } from './zipcode.pipe';
import { DocumentPipe } from './document.pipe';
import { PhonePipe } from './phone.pipe';
import { NgModule } from '@angular/core';
import { OrderByPipe } from './order-by.pipe';


@NgModule({
    declarations: [
        ZipcodePipe,
        DocumentPipe,
        PhonePipe,
        OrderByPipe
    ],
    exports: [
        ZipcodePipe,
        DocumentPipe,
        PhonePipe,
        OrderByPipe]
})
export class PipesModule { }