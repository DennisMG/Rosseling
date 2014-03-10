'use strict';

// Google Analytics Collection APIs Reference:
// https://developers.google.com/analytics/devguides/collection/analyticsjs/

angular.module('app.controllers', [])

    // Path: /
    .controller('HomeCtrl', [
        '$scope', '$location', '$window', function($scope, $location, $window) {
            $scope.$root.title = 'MiniTrello';
            $scope.$on('$viewContentLoaded', function() {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ])

    // Path: /error/404
    .controller('Error404Ctrl', [
        '$scope', '$location', '$window', function($scope, $location, $window) {
            $scope.$root.title = 'Error 404: Page Not Found';
            $scope.$on('$viewContentLoaded', function() {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ])

    .controller('AccountController', [
        '$scope', '$location', '$window', 'AccountServices', 'rcMailgun', function ($scope, $location, $window, AccountServices, rcMailgun) {

            $scope.hasError = false;
            $scope.errorMessage = '';

           

            $scope.isLogged = function() {
                return $window.sessionStorage.token != null;
            };

            $scope.loginModel = { Email: '', Password: '' };

            $scope.registerModel = { Email: '', Password: '', FirstName: '', LastName: '', ConfirmPassword: '' };

            
            $scope.login = function() {
                console.log($scope.loginModel);
                AccountServices
                    .login($scope.loginModel)
                    .success(function(data, status, headers, config) {

                        $window.sessionStorage.token = data.Token;
                        //$location.path('/boards');
                    })
                    .error(function(data, status, headers, config) {
                        // Erase the token if the user fails to log in
                        delete $window.sessionStorage.token;

                        $scope.errorMessage = 'Error o clave incorrect';
                        $scope.hasError = true;
                        // Handle login errors here
                        $scope.message = 'Error: Invalid user or password';
                    });
                //$location.path('/');
            };

            $scope.goToRegister = function() {
                $location.path('/register');
            };

            $scope.goToLogin = function() {
                $location.path('/login');
            };

            $scope.register = function () {
                console.log($scope.registerModel);
                AccountServices
                    .register($scope.registerModel)
                    .success(function(data, status, headers, config) {
                        console.log(data);
                        $scope.goToLogin();
                        
                    })
                    .error(function(data, status, headers, config) {
                        console.log(data);
                    });
            };

            $scope.$on('$viewContentLoaded', function() {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ]);


