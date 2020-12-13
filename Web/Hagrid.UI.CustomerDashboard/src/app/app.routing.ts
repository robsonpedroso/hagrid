import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuardService } from "./shared/auth/auth-guard.service";
import { AppComponent } from "./app.component";
import { HomeModule } from "./home/home.module";
import { Error404Component } from "./errors/error-404/error-404.component";
import { Error403Component } from "./errors/error-403/error-403.component";
import { MyDataComponent } from "./admin/my-data/my-data.component";

const routes: Routes = [
  {
    path: "",
    redirectTo: "/home",
    pathMatch: "full"
    //canActivate: [AuthGuardService]
  },
  {
    path: "home",
    loadChildren: "./home/home.module#HomeModule"
    //canActivate: [AuthGuardService]
  },
  {
    path: "dashboard",
    loadChildren: "./admin/dashboard/dashboard.module#DashboardModule",
    canActivate: [AuthGuardService]
  },
  {
    path: "meus-dados",
    loadChildren: "./admin/my-account/my-account.module#MyAccountModule",
    canActivate: [AuthGuardService]
  },
  {
    path: "meus-cartoes",
    loadChildren: "./admin/my-cards/my-cards.module#MyCardsModule",
    canActivate: [AuthGuardService]
  },
  {
    path: "superpoints",
    loadChildren: "./admin/superpoints/superpoints.module#SuperpointsModule",
    canActivate: [AuthGuardService]
  },
  {
    path: "authorized/:transfer_token",
    component: AppComponent,
    canActivate: [AuthGuardService],
    data: { type: "authorized" }
  },
  {
    path: "forbidden",
    component: Error403Component
  },
  {
    path: "**",
    component: Error404Component
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  declarations: [],
  exports: [RouterModule]
})
export class AppRoutingModule {}
