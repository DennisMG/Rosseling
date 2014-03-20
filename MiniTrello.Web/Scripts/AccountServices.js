'use strict';

angular.module('app.services', []).factory('AccountServices', ['$http', function ($http) {

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

    account.forgotPassword = function (data) {
        return $http.post('http://minitrelloapidm.apphb.com/forgotpassword/' + $scope.sessionStorage.token, data);
    };

    return account;



}]);


