'use strict';

angular.module('app.services', []).factory('AccountServices', ['$http', function ($http) {

    var account = {};

    account.login = function (data) {
        return $http.post('http://minitrelloapidm.apphb.com/login', data);
    };

    account.register = function (data) {
        return $http.post('http://minitrelloapidm.apphb.com/register', data);
    };

    return account;

}]);