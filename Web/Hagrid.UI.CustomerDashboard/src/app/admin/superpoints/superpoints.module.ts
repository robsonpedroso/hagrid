import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SuperpointsService } from '../../shared/services/superpoints.service';
import { SuperpointsRoutingModule } from './superpoints-routing.module';
import { SuperpointsComponent } from '../superpoints/superpoints.component';
import {MatTabsModule, MatDatepickerModule, MatFormFieldModule, MatInputModule} from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {MomentDateAdapter} from '@angular/material-moment-adapter';
import { SuperpointsHomeComponent } from './superpoints-home/superpoints-home.component';
import { SuperpointsBoxComponent } from './superpoints-box/superpoints-box.component';

@NgModule({
  imports: [
    CommonModule,
    SuperpointsRoutingModule,
    MatTabsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatMomentDateModule,
    MatInputModule,
    NgxPaginationModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [SuperpointsComponent, SuperpointsHomeComponent, SuperpointsBoxComponent],
  providers: [
    SuperpointsService,
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]}
  ],
  exports: [SuperpointsBoxComponent]
})
export class SuperpointsModule { }
