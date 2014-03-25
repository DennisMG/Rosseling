'use strict';

angular.module('app.services').factory('OrganizationServices', ['$http', '$window', function ($http, $window) {

    var organization = {};

    var baseRemoteUrl = "http://minitrelloapidm.apphb.com";
    var baseLocalUrl = "http://localhost:1416";
    var baseUrl = baseRemoteUrl;

    organization.getOrganizationsForLoggedUser = function () {
        return $http.get(baseUrl + '/organizations/' + $window.sessionStorage.token);
    };

    organization.createOrganizationsForLoggedUser = function (data) {
        return $http.post(baseUrl + '/organization/addorganization/' + $window.sessionStorage.token,data);
    };

    organization.deleteOrganization = function(data) {
        return $http.delete('minitrelloapidm.apphb.com/organization/' + $window.sessionStorage.token, data);
    };

    return organization;

}]);