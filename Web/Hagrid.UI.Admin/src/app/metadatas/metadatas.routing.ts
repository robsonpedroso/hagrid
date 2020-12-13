import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MetadatasComponent } from './metadatas.component';
import { MetadataFieldComponent } from './metadata-field/metadata-field.component';
import { AuthPermissionService } from '../shared/auth/auth-permission.service';
import { Keys } from '../shared/models/keys';

const routes: Routes = [
    {
        path: '', component: MetadatasComponent,
        children: [
            { path: '', component: MetadataFieldComponent, canActivate: [AuthPermissionService], data: { role: Keys.MetadadosModule.Metadados }   }
        ]
    }];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class MetadatasRoutingModule { }
