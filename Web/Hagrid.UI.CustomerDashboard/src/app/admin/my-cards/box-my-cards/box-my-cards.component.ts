import { Observable } from "rxjs/Observable";
import { Component, OnInit } from "@angular/core";
import { CardsService } from "../../../shared/services/cards.service";
import { ActivatedRoute } from "@angular/router";
import { ToolsService } from "../../../shared/services/core";

@Component({
  selector: 'app-box-my-cards',
  templateUrl: './box-my-cards.component.html',
  styleUrls: ['./box-my-cards.component.scss']
})
export class BoxMyCardsComponent implements OnInit {
  cards: any[];
  is_loaded: boolean;
  cardDetail: any = {};

  constructor(
    private cardsService: CardsService,
    private route: ActivatedRoute,
    private tools: ToolsService
    ) {}

  ngOnInit() {
    this.getCards();
  }

  public getCards(){
    this.cardsService.get().subscribe(response => {

      response.forEach((card) => {
        card.brand_icon = this.tools.getBrand(card.brand);
      });

      this.cards = response;
      if (this.cards.length > 0)
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

  public inactiveCard(){
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
}
