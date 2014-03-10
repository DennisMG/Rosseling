'use strict';

angular.module('app.services', ['rcMailgun']).factory('AccountServices', ['$http', function ($http) {

    var account = {};

    account.login = function (data) {
        return $http.post('http://minitrelloapidm.apphb.com/login', data);
    };

    account.register = function (data) {
        return $http.post('http://minitrelloapidm.apphb.com/register', data);
    };

    return account;



}]);

angular.module('LoginApp', ['rcMailgun'])

.config(['rcMailgunProvider', function (rcMailgunProvider) {

    var mailgunOptions = {
        api_key: 'key-44dmt83s6yknc9v5b8qatxj45cqpujm5',
        in_progress: null,
        success: null,
        error: null,
    };

    rcMailgunProvider.configure(mailgunOptions);
}]);
