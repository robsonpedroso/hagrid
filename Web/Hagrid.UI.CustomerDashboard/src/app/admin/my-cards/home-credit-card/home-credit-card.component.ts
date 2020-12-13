import { Observable } from "rxjs/Observable";
import { Component, OnInit } from "@angular/core";
import { CardsService } from "../../../shared/services/cards.service";
import { ActivatedRoute } from "@angular/router";
import { ToolsService } from "../../../shared/services/core";
import { CustomerService } from "../../../shared/services/customer.service";

@Component({
  selector: "app-home-credit-card",
  templateUrl: "./home-credit-card.component.html",
  styleUrls: ['./home-credit-card.component.scss']
})
export class HomeCreditCardComponent implements OnInit {
  cards: any[];
  cardDetail: any = {};
  is_loaded: boolean;
  addressDetail: any = {}

  constructor(
    private cardsService: CardsService,
    private route: ActivatedRoute,
    private tools: ToolsService,
    private cusomterService: CustomerService) { }

  ngOnInit() {
    this.getCards();
    this.getAddress();
  }

  public getCards() {
    this.cardsService.get().subscribe(response => {

      response.forEach((card) => {
        card.brand_icon = this.tools.getBrand(card.brand);
      });

      this.cards = response;
      this.is_loaded = true;
    });
  }

  public activeCard(cardSelected: any) {
    let card = document.querySelector('.cards__master');
    card.classList.remove('zero');
    card.classList.remove('inactive');
    card.classList.add('active');
    this.cardDetail = cardSelected;
  }

  public inactiveCard() {
    let card = document.querySelector('.cards__master');
    card.classList.remove('active');
    card.classList.add('inactive');
    setTimeout(() => {
      card.classList.add('zero');
      this.cardDetail = {};
    }, 500);
  }

  public delete(code: string): void {
    this.cardsService.delete(code).subscribe(
      (response) => {
        this.getCards();
      }
    );
  }

  public getAddress() {
    this.cusomterService.getAddresses().subscribe(response => {
      if (response.length > 0) {
        response.filter

        let flag = false;
        response.forEach(address => {
          if (address.purpose.toLowerCase() == 'shipping') {
            address.phones = address.phones.filter(x => x.number != '');
            this.addressDetail = address;
            flag = true;
          }
        });

        if (!flag)
        {
          this.addressDetail = response.find(x => x.purpose.toLowerCase() == 'contact');
        }
      }
    });
  }
}
