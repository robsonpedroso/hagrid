import { CreditCardValidatorService } from "./../../../shared/services/credit-card-validator.service";
import { MaskService } from "./../../../shared/services/mask.service";
import { Observable } from "rxjs/Observable";
import { Component, OnInit } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { FormControl, Validators, AbstractControl } from "@angular/forms";
import { NotificationsService } from "angular2-notifications";
import { ActivatedRoute, Router } from "@angular/router";
import { CardsService } from "../../../shared/services/cards.service";

declare var RPay: any;

@Component({
  selector: "app-new-credit-card",
  templateUrl: "./new-credit-card.component.html",
  styleUrls: ['./new-credit-card.component.scss']
})
export class NewCreditCardComponent implements OnInit {
  rkPayments: any = {};
  credit_card: any = {};
  rPay: any = {};
  is_loaded: boolean;

  constructor(
    private notify: NotificationsService,
    private route: ActivatedRoute,
    private cardsService: CardsService,
    private router: Router,
    private creditCardValidator: CreditCardValidatorService
  ) {}

  ngOnInit() {
    this.rPay = new RPay();
    this.is_loaded = true;
  }

  public documentMask(rawValue) {
    return MaskService.documentMask(rawValue);
  }

  public getBrand() {
    if (this.credit_card.card_number != null) {
      let credit_card_infos = this.creditCardValidator.validate(
        this.credit_card.card_number
      );

      if (credit_card_infos.luhn_valid && credit_card_infos.length_valid) {
        this.credit_card.brand = credit_card_infos.card_type.name;

        switch (this.credit_card.brand) {
          case "visa":
            this.credit_card.brand_icon = "fab fa-cc-visa";
            break;
          case "mastercard":
            this.credit_card.brand_icon = "fab fa-cc-mastercard";
            break;
          case "amex":
            this.credit_card.brand_icon = "fab fa-cc-amex";
            break;
          case "jcb":
            this.credit_card.brand_icon = "fab fa-cc-jcb";
            break;
          case "discover":
            this.credit_card.brand_icon = "fab fa-cc-discover";
            break;
          case "diners":
            this.credit_card.brand_icon = "fab fa-cc-diners-club";
            break;
          default:
            this.credit_card.brand_icon = "";
            break;
        }
      }
    }
  }

  public saveCreditCard() {
    try {
      let card = this.makePaymentCreditCard();

      this.rPay.tokenize(card, (error, data) => {
        if (error != null) {
          this.notify.error("Atenção!", error.message);
        } else if (data != null) {
          this.credit_card.signature = data.signature;
          this.credit_card.timestamp = data.timestamp;
          this.credit_card.brand = data.cardBrand;
          this.credit_card.card_token = data.cardToken;
          this.credit_card.expiration_month = data.cardExpirationMonth;
          this.credit_card.expiration_year = data.cardExpirationYear;
          this.credit_card.card_last_digits = data.cardNumberMasked.substr(-4);
          this.credit_card.card_bin = data.cardNumberMasked.substr(0, 6);

          this.cardsService.post(this.credit_card).subscribe(response => {
            this.notify.success("Sucesso", "Cartão inserido com sucesso.", {
              timeOut: 2500,
              showProgressBar: true,
              pauseOnHover: false,
              clickToClose: false
            });

            setTimeout(() => {
              this.router.navigate(["meus-cartoes"]);
            }, 2800);
          });
        }
      });
    } catch (e) {
      this.notify.error("Atenção", e);
    }
  }

  private makePaymentCreditCard() {
    if (
      this.credit_card.card_number == null ||
      this.credit_card.card_number == ""
    ) {
      throw new Error("Número do cartão de crédito não informado");
    }

    if (
      this.credit_card.holder_name == null ||
      this.credit_card.holder_name == ""
    ) {
      throw new Error("Nome do portador não informado");
    }

    if (
      this.credit_card.holder_document == null ||
      this.credit_card.holder_document == ""
    ) {
      throw new Error("Documento do titular não informado");
    }

    if (
      this.credit_card.expiration_month == null ||
      this.credit_card.expiration_month == ""
    ) {
      throw new Error("Mês de expiração não informado");
    }

    if (
      this.credit_card.expiration_year == null ||
      this.credit_card.expiration_year == ""
    ) {
      throw new Error("Ano de expiração não informado");
    }

    var data = {
      form: document.querySelector("form[data-rkp='form']"),
      "card-number": document.querySelector("input[data-rkp='card-number']"),
      "card-cvv": document.querySelector("input[data-rkp='card-cvv']"),
      "expiration-month": document.querySelector(
        "input[data-rkp='card-expiration-month']"
      ),
      "expiration-year": document.querySelector(
        "input[data-rkp='card-expiration-year']"
      )
    };

    return data;
  }
}
