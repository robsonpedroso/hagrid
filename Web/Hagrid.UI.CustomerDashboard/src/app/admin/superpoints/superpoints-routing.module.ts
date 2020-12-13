import { SuperpointsComponent } from './superpoints.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SuperpointsHomeComponent } from './superpoints-home/superpoints-home.component';


const routes: Routes = [
  {
    path: '', component: SuperpointsComponent,
    children: [
      { path: '', component: SuperpointsHomeComponent }
    ]
 }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SuperpointsRoutingModule { }
