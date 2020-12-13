/*
jQuery Credit Card Validator

Copyright 2012 Pawel Decowski

This work is licensed under the Creative Commons Attribution-ShareAlike 3.0
Unported License. To view a copy of this license, visit:

http://creativecommons.org/licenses/by-sa/3.0/

or send a letter to:

Creative Commons, 444 Castro Street, Suite 900,
Mountain View, California, 94041, USA.
*/

(function () {
    var $,
    __indexOf = Array.prototype.indexOf || function (item) { for (var i = 0, l = this.length; i < l; i++) { if (i in this && this[i] === item) return i; } return -1; };

    $ = jQuery;

    $.fn.validateCreditCard = function (callback) {
        var card_types, get_card_type, is_valid_length, is_valid_luhn, normalize, validate, validate_number;
        card_types = [
      {
          name: 'elo',
          pattern: /^((((636368)|(438935)|(504175)|(451416)|(636297))\d{0,10})|((5067)|(4576)|(4011))\d{0,12})$/,
          valid_length: [16]
      }, {
          name: 'amex',
          full_name: 'american express',
          pattern: /^([34|37]{2})([0-9]{13})$/,
          valid_length: [15]
      }, {
          name: 'diners',
          pattern: /^3[0|6|8][0-9]{12}$/,
          valid_length: [14]
      }, {
          name: 'visa',
          pattern: /^([4]{1})([0-9]{12,15})$/,
          valid_length: [16]
      }, {
          name: 'mastercard',
          pattern: /^5[1-5]\d{14}$|^(222[1-9]|2[3-6]\d{2}|27[0-1]\d|2720)\d{12}$/,
          valid_length: [16]
      }, {
          name: 'jcb',
          pattern: /^35(?:2[89]|[3-8]\d)\d{12}$/,
          valid_length: [16]
      }
      , {
          name: 'hipercard',
          pattern: /^(606282\d{10}(\d{3})?)|(3841\d{15})$/,
          valid_length: [16]
      }
        ];

        var dicEloBins = {
            "401178": "401179",
            "431274": "431274",
            "438935": "438935",
            "457393": "457393",
            "457631": "457632",
            "504175": "504175",
            "506707": "506708",
            "506715": "506715",
            "506717": "506721",
            "506723": "506733",
            "506739": "506748",
            "506753": "506753",
            "506774": "506775",
            "506777": "506778",
            "509000": "509002",
            "509004": "509014",
            "509020": "509064",
            "509066": "509072",
            "509074": "509092",
            "509095": "509103",
            "509105": "509807",
            "636297": "636297",
            "636368": "636368",
            "650405": "650439",
            "650485": "650538",
            "650552": "650598",
            "650720": "650727",
            "650901": "650922",
            "650928": "650928",
            "650939": "650939",
            "650946": "650978",
            "651652": "651704",
            "655000": "655019",
            "655021": "655058"

        };

        is_elo = function (number) {
            var bin = parseInt(number.substring(0, 6));
            var keys = Object.keys(dicEloBins);

            for (_i = 0; _i < keys.length; _i++) {

                var min = keys[_i];
                var max = dicEloBins[keys[_i]];

                if (bin >= min && bin <= max)
                    return true;
            }

            return false;
        };
        get_card_type = function (number) {
            var card_type, _i, _len;

            if (is_elo(number))
                return card_types[0];

            for (_i = 0, _len = card_types.length; _i < _len; _i++) {
                card_type = card_types[_i];
                if (number.match(card_type.pattern)) return card_type;
            }
            return null;
        };
        is_valid_luhn = function (number) {
            var digit, n, sum, _len, _ref;
            sum = 0;
            _ref = number.split('').reverse().join('');
            for (n = 0, _len = _ref.length; n < _len; n++) {
                digit = _ref[n];
                digit = +digit;
                if (n % 2) {
                    digit *= 2;
                    if (digit < 10) {
                        sum += digit;
                    } else {
                        sum += digit - 9;
                    }
                } else {
                    sum += digit;
                }
            }
            return sum % 10 === 0;
        };
        is_valid_length = function (number, card_type) {
            var _ref;
            return _ref = number.length, __indexOf.call(card_type.valid_length, _ref) >= 0;
        };
        validate_number = function (number) {
            var card_type, length_valid, luhn_valid;
            card_type = get_card_type(number);
            luhn_valid = false;
            length_valid = false;
            if (card_type != null) {
                luhn_valid = is_valid_luhn(number);
                length_valid = is_valid_length(number, card_type);
            }
            return callback({
                card_type: card_type,
                luhn_valid: luhn_valid,
                length_valid: length_valid
            });
        };
        validate = function () {

            var number;
            number = normalize($(this).val());
            return validate_number(number);
        };
        normalize = function (number) {
            return number.replace(/[ -]/g, '');
        };
        this.bind('input', function () {
            $(this).unbind('keyup');
            return validate.call(this);
        });
        this.bind('keyup', function () {
            return validate.call(this);
        });
        validate.call(this);
        return this;
    };

}).call(this);

function CreditCardIsExpired(yearExpiration, monthExpiration) {

    var today = new Date();

    return !(yearExpiration > today.getFullYear() || (yearExpiration == today.getFullYear() && monthExpiration > (today.getMonth() + 1)));
};
