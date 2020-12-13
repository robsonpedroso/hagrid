'use strict';
accountsApp.controller('creditCardController', ['$scope', '$routeParams', '$sessionStorage', '$location', 'authService', 'creditCardService',
    function ($scope, $routeParams, $sessionStorage, $location, authService, creditCardService) {

        $scope.token;
        $scope.creditcard = {};
        $scope.isLoaded = false;
        $scope.visibleMessage = null;
        $scope.message = null;
        $scope.messageClass = null;
        $scope.phrase = null;
        $scope.invalidCreditCardBrand = '';

        $scope.getSecretPhrase = function () {
            creditCardService.getSecretPhrase($routeParams.phrase).then(function (response) {
                $scope.phrase = response.data.content;
            });
        };

        $scope.validateCreditCard = function (data) {

            if($scope.creditcard != null && $scope.creditcard.number != ''){

                $('#credit-card-number').validateCreditCard(function (result) {

                    if(result.card_type != null && result.card_type.name != 'mastercard' && result.card_type.name != 'visa' && result.card_type.name != 'amex'
                    && result.card_type.name != 'diners' && result.card_type.name != 'elo' && result.card_type.name != 'hipercard'){
                        $scope.invalidCreditCardBrand = 'Cartão de crédito inválido ou bandeira não aceita.';
                        $scope.creditCardForm.number.$error = { invalid_credit_card_brand: true };
                        $scope.creditCardForm.number.$invalid = { invalid_credit_card_brand: true };
                    } else if (result.card_type == null) {
                        $scope.invalidCreditCardBrand = 'Cartão de crédito inválido ou bandeira não aceita.';
                        $scope.creditCardForm.number.$error = { invalid_credit_card_brand: true };
                        $scope.creditCardForm.number.$invalid = { invalid_credit_card_brand: true };
                    } else {
                        $scope.invalidCreditCardBrand = '';
                        $scope.creditCardForm.number.$error = false;
                        $scope.creditCardForm.number.$invalid = false;
                    }
                });
            } else {
                $scope.invalidCreditCardBrand = '';
            }
        };

        $scope.save = function () {

            if($scope.invalidCreditCardBrand == ''){

                var expiration_date = $scope.creditcard.expiration_date;
                var vallues = expiration_date.split('/');

                if (expiration_date.length > 0) {
                    $scope.creditcard.expiration_month = vallues[0];
                }

                if (expiration_date.length >= 1) {
                    $scope.creditcard.expiration_year = vallues[1];
                }

                $scope.creditcard.store_code = $routeParams.store;

                creditCardService.save($scope.creditcard).then(function (response) {

                    $scope.messageClass = "alert-success";
                    $scope.message = 'Seus dados foram enviados com sucesso.';
                    $scope.visibleMessage = true;
                    $scope.isLoaded = true;
                    $scope.creditcard = {};
                    $scope.creditCardForm.$setUntouched();
                    $scope.creditCardForm.$setPristine();
                },
                function (err) {

                    $scope.messageClass = "alert-danger";
                    $scope.message = err.data.messages[0].text;
                    $scope.visibleMessage = true;
                    $scope.isLoaded = true;
                });
            } else {
                $scope.messageClass = "alert-danger";
                $scope.message = "Informe um cartão de crédito válido";
                $scope.visibleMessage = true;
                $scope.isLoaded = true;
            }
        };


        $scope.init = function () {
            if (!$routeParams.store) {
                $location.path("notfound");
            }
            else {
                if (!$sessionStorage.AuthToken) {
                    authService.getClientCredentialToken().then(function (response) {
                        if (typeof (response.access_token) !== "undefined" && response.access_token != "") {

                            $scope.getSecretPhrase();
                            $scope.isLoaded = true;
                        }
                    },
                    function (err) {
                        $scope.isLoaded = false;
                    });
                }
                else {
                    $scope.getSecretPhrase();
                    $scope.isLoaded = true;
                }
            }
        };
    }]);