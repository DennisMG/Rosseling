'use strict';

angular.module('app.services').factory('LaneServices', ['$http', '$window', function ($http, $window) {

    var lane = {};

    var baseRemoteUrl = "http://minitrelloapidm.apphb.com";
    var baseLocalUrl = "http://localhost:1416";
    var baseUrl = baseRemoteUrl;

    lane.getOrganizationsForLoggedUser = function () {
        return $http.get(baseUrl + '/organizations/' + $window.sessionStorage.token);
    };

    lane.createOrganizationsForLoggedUser = function (data) {
        return $http.post(baseUrl + '/organization/addorganization/' + $window.sessionStorage.token, data);
    };

    return lane;

}]);