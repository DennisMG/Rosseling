'use strict';

angular.module('app.services').factory('BoardServices', ['$http', '$window', function ($http, $window) {

    var board = {};

    var baseRemoteUrl = "http://minitrelloapidm.apphb.com";
    var baseLocalUrl = "http://localhost:1416";
    var baseUrl = baseRemoteUrl;

    board.getBoardsForLoggedUser = function (IdOrganization) {
        return $http.get(baseUrl + '/getboards/' + IdOrganization + '/' + $window.sessionStorage.token);
    };

    board.createBoardsForLoggedUser = function (data, IdOrganization) {
        return $http.post(baseUrl + '/createBoard/' + IdOrganization + '/' + $window.sessionStorage.token,data);
    };

    return board;

}]);