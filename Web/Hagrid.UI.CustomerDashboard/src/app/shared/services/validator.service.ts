import { AbstractControl, ValidatorFn, Validators, FormControl, ValidationErrors} from "@angular/forms";


export class ValidatorService {

  constructor() {}

  public static validate(control: AbstractControl, fieldType: string)
  {
    return this[fieldType](control);
  }

  public static requiredByCustomer(isPerson: boolean, type: string): ValidatorFn {

    return (control: AbstractControl): { [key: string]: any } => {
      const value = control.value;

      switch (type) {
        case "person":
          if (isPerson && (!value || !value.length)) {
            return { required_by_customer: true };
          }
          break;

        case "company":
          if (!isPerson && (!value || !value.length)) {
            return { required_by_customer: true };
          }
          break;
      }

      return null;
    };
  }

  public static currentPassword(control: AbstractControl) {
    if(control.value == null || control.value == ""){
      return { required_password: true };
    } else {
      return null;
    }
  }

  public static newPassword(control: AbstractControl) {
    if(control.value == null || control.value == ""){
      return { required_new_password: true };
    } else if (control.value.length < 8) {
      return { min_length_new_password: true };
    } else {
      return null;
    }
  }

  public static confirmPassword(control: AbstractControl) {

    let newPassword = control.parent.controls["new_password"].value;
    let confirmPassword = control.value;

    if(confirmPassword == null || confirmPassword == ""){
      return { required_confirm_password: true };
    } else if (newPassword != confirmPassword) {
      return { invalid_confirm_password: true };
    } else {
      return null;
    }
  }

  public static email(control: AbstractControl): ValidationErrors | null {
    let email = control.value;
    let emailRegex = /^[a-z0-9!#$%&'*+\/=?^_`{|}~.-]+@[a-z0-9]([a-z0-9-]*[a-z0-9])?(\.[a-z0-9]([a-z0-9-]*[a-z0-9])?)*$/i;

    return emailRegex.test(email)
      ? null
      : {
          invalid_email: true
        };
  }

  public static validateCreditCard(control: AbstractControl, service: any) {

    if (control.value == null || control.value == "") {
      return { required_creditcard_number: true };
    } else {

      let credit_card_infos = service.validate(control.value);

      if (credit_card_infos.luhn_valid && credit_card_infos.length_valid) {

        switch (credit_card_infos.card_type.name) {
          case "visa":
          case "mastercard":
          case "amex":
          case "elo":
          case "hipercard":
          case "diners":
            return  null;
          default:
          return { invalid_creditcard_brand: true };
        }
      } else {
        return { invalid_creditcard_number: true };
      }
    }
  }

  private static creditcardHolderName(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_creditcard_holder_name: true };
    } else {
      return null;
    }
  }

  private static document(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_document: true };
    } else {

      let document = control.value.replace(/[^\d]+/g,'');

      if(document.length > 11) {
        return this.validateCnpj(document) ? null : { invalid_document: true };
      } else {
        return this.validateCpf(document) ? null : { invalid_document: true };
      }
    }
  }

  public static validateCpf(cpf: string): boolean {
    if (cpf == null) {
        return false;
    }
    if (cpf.length != 11) {
        return false;
    }
    if ((cpf == '00000000000') || (cpf == '11111111111') || (cpf == '22222222222') || (cpf == '33333333333') || (cpf == '44444444444') || (cpf == '55555555555') || (cpf == '66666666666') || (cpf == '77777777777') || (cpf == '88888888888') || (cpf == '99999999999')) {
        return false;
    }
    let numero: number = 0;
    let caracter: string = '';
    let numeros: string = '0123456789';
    let j: number = 10;
    let somatorio: number = 0;
    let resto: number = 0;
    let digito1: number = 0;
    let digito2: number = 0;
    let cpfAux: string = '';
    cpfAux = cpf.substring(0, 9);
    for (let i: number = 0; i < 9; i++) {
        caracter = cpfAux.charAt(i);
        if (numeros.search(caracter) == -1) {
            return false;
        }
        numero = Number(caracter);
        somatorio = somatorio + (numero * j);
        j--;
    }
    resto = somatorio % 11;
    digito1 = 11 - resto;
    if (digito1 > 9) {
        digito1 = 0;
    }
    j = 11;
    somatorio = 0;
    cpfAux = cpfAux + digito1;
    for (let i: number = 0; i < 10; i++) {
        caracter = cpfAux.charAt(i);
        numero = Number(caracter);
        somatorio = somatorio + (numero * j);
        j--;
    }
    resto = somatorio % 11;
    digito2 = 11 - resto;
    if (digito2 > 9) {
        digito2 = 0;
    }
    cpfAux = cpfAux + digito2;
    if (cpf != cpfAux) {
        return false;
    }
    else {
        return true;
    }
  }

  public static validateCnpj(cnpj: string): boolean {
    cnpj = cnpj.replace(/[^\d]+/g,'');

    if(cnpj == '') {
      return false;
    }

    if (cnpj.length != 14) {
        return false;
    }


    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999")
        return false;


    let tamanho = cnpj.length - 2
    let numeros = cnpj.substring(0,tamanho);
    let digitos = cnpj.substring(tamanho);
    let soma = 0;
    let pos = tamanho - 7;
    for (let i = tamanho; i >= 1; i--) {
      soma += +numeros.charAt(tamanho - i) * pos--;
      if (pos < 2)
            pos = 9;
    }
    let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != +digitos.charAt(0))
        return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0,tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (let i = tamanho; i >= 1; i--) {
      soma += +numeros.charAt(tamanho - i) * pos--;
      if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != +digitos.charAt(1))
          return false;

    return true;
  }

  private static creditcardMonth(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_creditcard_month: true };
    } else if(control.value.length != 2 || +control.value < 1  || +control.value > 12) {
      return { invalid_creditcard_month: true };
    } else {
      return null;
    }
  }

  private static creditcardYear(control: AbstractControl) {

    if (control.value == null || control.value == "") {
      return { required_creditcard_year: true };
    } else if(control.value.length != 4 || +control.value < (new Date()).getFullYear()) {
      return { invalid_creditcard_year: true };
    } else {
      return null;
    }
  }

  private static creditcardCvv(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_cvv: true };
    } else {
      return null;
    }
  }

  private static addressLocationName(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_address_location_name: true };
    } else {
      return null;
    }
  }

  private static addressReceiverName(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_address_receiver_name: true };
    } else {
      return null;
    }
  }

  private static zipCode(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_zip_code: true };
    } else {
      return null;
    }
  }

  private static address(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_address: true };
    } else {
      return null;
    }
  }

  private static addressNumber(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_address_number: true };
    } else {
      return null;
    }
  }

  private static addressDistrict(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_district: true };
    } else {
      return null;
    }
  }

  private static city(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_city: true };
    } else {
      return null;
    }
  }

  private static state(control: AbstractControl) {
    if (control.value == null || control.value == "") {
      return { required_state: true };
    } else {
      return null;
    }
  }

  private static phone(control: AbstractControl) {

    if (control.value == null || control.value == "") {
      return { required_phone: true };
    } else {
      let phone = control.value.replace(/\(|\)|\_|\ |\-/gi, "")

      if(phone == "" || phone.length < 10){
        return { invalid_phone: true };
      }

      return null;
    }
  }

  private static phoneNotRequired(control: AbstractControl) {

    if (control.value == null || control.value == "") {
      return null;
    } else {
      let phone = control.value.replace(/\(|\)|\_|\ |\-/gi, "")

      if(phone == "" || phone.length < 10){
        return { invalid_phone: true };
      }

      return null;
    }
  }

  public static documentValidate(control: AbstractControl){
    if(control.value == null || control.value == "")
      return { required_document: true };

    if(control.value == "invalido")
      return { invalid_document: true };
  }

  public static nameValidate(control: AbstractControl){
    if(control.value == null || control.value == "")
      return { required_name: true };
  }

  public static lastNameValidate(control: AbstractControl){
    if(control.value == null || control.value == "")
      return { required_lastname: true };
  }

  public static namePJValidate(control: AbstractControl){
    if(control.value == null || control.value == "")
      return { required_name_pj: true };
  }

  public static fantasyNameValidate(control: AbstractControl){
    if(control.value == null || control.value == "")
      return { required_fantasy_name: true };
  }

  public static birthday(control: AbstractControl){

let minAge = new Date();
minAge.setFullYear(+minAge.getFullYear() - 85);

    if(control.value == null || control.value == ""){
    return { required_birthday: true };
    } else if (control.value > (new Date()).getDate() || control.value < minAge){
      return { invalid_birthday: true };

    }
  }

}
