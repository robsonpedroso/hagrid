import { LogisticsService } from './logistics.service';
import { Injectable } from "@angular/core";
import { NotificationsService } from 'angular2-notifications';

@Injectable()
export class ToolsService {

  constructor() { }

  public setPhone(phone: any) {

    if (phone.ddd != "" && phone.number != "") {
      return phone.ddd + " " + phone.number;
    } else {
      return "";
    }
  }

  public getPhone(phoneNumber: string, phoneType) {
    let phone: any;

    if (typeof phoneNumber != "undefined" && phoneNumber != "") {
      phone = {
        code_country: "55",
        phone_type: phoneType,
        ddd: phoneNumber.replace("(", "").replace(")", "").substring(0, 2),
        number:
          phoneNumber.length == 14
            ? phoneNumber.replace("(", "").replace(")", "").replace(" ","").replace("-", "").substring(2, 10)
            : phoneNumber.replace("(", "").replace(")", "").replace(" ","").replace("-", "").substring(2, 11),
        extension: ""
      };
    } else {
      phone = {
        code_country: "",
        phone_type: phoneType,
        ddd: "",
        number: "",
        extension: ""
      };
    }

    return phone;
  }

  public getStates() {
    return [
      {
        id: "AC",
        text: "AC"
      },
      {
        id: "AL",
        text: "AL"
      },
      {
        id: "AP",
        text: "AP"
      },
      {
        id: "AM",
        text: "AM"
      },
      {
        id: "BA",
        text: "BA"
      },
      {
        id: "CE",
        text: "CE"
      },
      {
        id: "DF",
        text: "DF"
      },
      {
        id: "ES",
        text: "ES"
      },
      {
        id: "GO",
        text: "GO"
      },
      {
        id: "MA",
        text: "MA"
      },
      {
        id: "MT",
        text: "MT"
      },
      {
        id: "MS",
        text: "MS"
      },
      {
        id: "MG",
        text: "MG"
      },
      {
        id: "PA",
        text: "PA"
      },
      {
        id: "PB",
        text: "PB"
      },
      {
        id: "PR",
        text: "PR"
      },
      {
        id: "PE",
        text: "PE"
      },
      {
        id: "PI",
        text: "PI"
      },
      {
        id: "RJ",
        text: "RJ"
      },
      {
        id: "RN",
        text: "RN"
      },
      {
        id: "RS",
        text: "RS"
      },
      {
        id: "RO",
        text: "RO"
      },
      {
        id: "RR",
        text: "RR"
      },
      {
        id: "SC",
        text: "SC"
      },
      {
        id: "SP",
        text: "SP"
      },
      {
        id: "SE",
        text: "SE"
      },
      {
        id: "TO",
        text: "TO"
      }
    ];
  }

  public getStatusDescription(status: string): string {
    let result = status;
    switch (status) {
      case "pending":
        result = "Aguardando autorização"
        break;
      case "authorized":
        result = "Aguardando confirmação de pagamento"
        break;
      case "approved":
        result = "Pagamento Confirmado"
        break;
      case "cancelled":
        result = "Cancelado"
        break;
      case "partial_refunded":
        result = "Estornado Parcialmente"
        break;
      case "refunded":
        result = "Estornado Totalmente"
        break;
      default:
        break;
    }

    return result;
  }

  public getPaymentsMethodDescription(method: string): string {
    let result = method;
    switch (method) {
      case "credit_card":
        result = "Cartão de Crédito"
        break;
      case "billet":
        result = "Boleto"
        break;
      case "balance":
        result = "Superpoints"
        break;
      case "store_credit":
        result = "Créditos Online"
        break;
      case "parcele":
        result = "Parcele"
        break;
      default:
        break;
    }

    return result;
  }


  public getBrand(brand: string): string {
    let brandIcon = "";

    switch (brand) {
      case "VISA":
      brandIcon = "cards__icon cards__icon--visa";
        break;
      case "MASTERCARD":
      brandIcon = "cards__icon cards__icon--master";
        break;
      case "AMEX":
      brandIcon = "cards__icon cards__icon--amex";
        break;
      case "JCB":
      brandIcon = "cards__icon cards__icon--jcb";
        break;
      case "DISCOVER":
      brandIcon = "cards__icon cards__icon--discover";
        break;
      case "DINERS":
      brandIcon = "cards__icon cards__icon--diners";
        break;
      default:
      brandIcon = "";
        break;
    }

    return brandIcon;
  }

}
