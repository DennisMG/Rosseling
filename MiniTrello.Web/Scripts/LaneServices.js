'use strict';

angular.module('app.services').factory('LaneServices', ['$http', '$window', function ($http, $window) {

    var lane = {};

    var baseRemoteUrl = "http://minitrelloapidm.apphb.com";
    var baseLocalUrl = "http://localhost:1416";
    var baseUrl = baseRemoteUrl;

    lane.getLanesForLoggedUser = function (IdBoard) {
        return $http.get(baseUrl + '/getlanes/' + IdBoard + '/' + $window.sessionStorage.token);
    };

    lane.createLanesForLoggedUser = function (data, IdBoard) {
        return $http.post(baseUrl + '/createlane/' + IdBoard + '/' + $window.sessionStorage.token, data);
    };

    lane.getCardsForLane = function ( LaneId) {
        return $http.get(baseUrl + '/getcards/' + LaneId + '/' + $window.sessionStorage.token);
    };

    lane.deleteCardsForLane = function (LaneId) {
        return $http.get(baseUrl + '/deletelane/' + LaneId + '/' + $window.sessionStorage.token);
    };

    return lane;

}]);