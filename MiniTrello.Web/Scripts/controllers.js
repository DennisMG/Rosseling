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

     .controller('BoardController', ['$scope', '$location', '$window', '$stateParams', function ($scope, $location, $window,  $stateParams) {


         $scope.boardDetailId = $stateParams.boardId;

         //console.log($location.search().boardId);

         console.log($scope.boardDetailId);

         $scope.boards = [];

         var board = { Id: 1, Name: 'Myboard1', Description: 'Description1' };
         var board1 = { Id: 2, Name: 'Myboard2', Description: 'Description2' };
         $scope.boards.push(board);
         $scope.boards.push(board1);


         $scope.getBoardsForLoggedUser = function () {

             BoardServices
                 .getBoardsForLoggedUser()
               .success(function (data, status, headers, config) {
                   console.log(data);
                   $scope.boards = data;
               })
               .error(function (data, status, headers, config) {
                   console.log(data);
               });
             //$location.path('/');
         };

         if ($scope.boardDetailId > 0) {
             //get board details
         }
         else {
             $scope.getBoardsForLoggedUser();
         }




         $scope.$on('$viewContentLoaded', function () {
             $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
         });
     }])

    .controller('AccountController', [
        '$scope', '$location', '$window', 'AccountServices', '$stateParams', function ($scope, $location, $window, AccountServices, $stateParams) {

    $scope.hasError = false;
    $scope.errorMessage = '';

    $scope.isLogged = function() {
        return $window.sessionStorage.token != null;
    };

    $scope.loginModel = { Email: '', Password: '' };
    $scope.changePasswordModel = { Email: '' };

    $scope.registerModel = { Email: '', Password: '', FirstName: '', LastName: '', ConfirmPassword: '' };
    $scope.AccountForgotPasswordModel = { Email: '', NewPassword: '',  ConfirmNewPassword: '' };
       
          
            
            $scope.login = function () {

                $scope.goToLoadingPage();
                console.log($scope.loginModel);

                AccountServices
                    .login($scope.loginModel)
                    .success(function(data, status, headers, config) {
                        $scope.goToLoadingPage();
                        $window.sessionStorage.token = data.Token;
                        $location.path('/boards');
                        
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

            $scope.goToLoadingPage = function () {
                $location.path('/loading');
            };

            $scope.goToLogin = function() {
                $location.path('/login');
            };

            $scope.register = function () {
                $scope.goToLoadingPage();
                console.log($scope.registerModel);
                AccountServices
                    .register($scope.registerModel)
                    .success(function (data, status, headers, config) {
                       
                        console.log(data);
                        
                        $scope.goToLogin();
                        
                    })
                    .error(function(data, status, headers, config) {
                        console.log(data);
                    });
            };

            $scope.sendEmail = function () {
                $scope.goToLoadingPage();
                AccountServices.sendEmail($scope.changePasswordModel)
                .success(function (data, status, headers, config) {
                    console.log(data);
                    $scope.goToLogin();

                })
                    .error(function (data, status, headers, config) {
                        console.log(data);
                    });

            };
        
            $scope.forgotpassword = function () {
                
                $scope.goToLoadingPage();
                //$scope.sessionStorage.token = $stateParams.token;
                console.log($stateParams.tokencito);
                console.log($scope.AccountForgotPasswordModel);
                AccountServices.forgotPassword($scope.AccountForgotPasswordModel, $stateParams.tokencito)
                    .success(function (data, status, headers, config) {

                        console.log(data);

                        $scope.goToLogin();

                    })
                    .error(function (data, status, headers, config) {
                        console.log(data);
                    });
            };
            

            $scope.$on('$viewContentLoaded', function() {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ]);




