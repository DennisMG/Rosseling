'use strict';

angular.module('app.services', []).factory('AccountServices', ['$http', '$window', function ($http,$window) {

    var account = {};

    account.login = function (data) {
        return $http.post('http://minitrelloapidm.apphb.com/login', data);
    };

    account.sendEmail = function (data) {
        return $http.post('http://minitrelloapidm.apphb.com/sendEmail', data);
    };

    account.register = function (data) {
        return $http.post('http://minitrelloapidm.apphb.com/register', data);
    };

    account.forgotPassword = function (token,data) {
        return $http.post('http://minitrelloapidm.apphb.com/forgotpassword/' + token, data);
    };

    account.updateAccount = function ( data) {
        return $http.post('http://minitrelloapidm.apphb.com/updateaccount/' + $window.sessionStorage.token, data);
    };

    account.getAccount = function () {
        return $http.get('http://minitrelloapidm.apphb.com/getaccount/' + $window.sessionStorage.token);
    };

    return account;



}]);


