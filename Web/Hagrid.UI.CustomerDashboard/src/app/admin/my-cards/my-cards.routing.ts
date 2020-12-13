import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { MyCardsComponent } from "./my-cards.component";
import { NewCreditCardComponent } from "./new-credit-card/new-credit-card.component";
import { HomeCreditCardComponent } from "./home-credit-card/home-credit-card.component";

const routes: Routes = [
  {
     path: '',
     component: MyCardsComponent,
    children: [
      { path: '', component: HomeCreditCardComponent },
      { path: 'novo', component: NewCreditCardComponent }]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyCardsRoutingModule {}
