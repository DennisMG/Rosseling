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
    .controller('BoardController', [
        '$scope', '$location', '$window', '$stateParams', 'BoardServices', function($scope, $location, $window, $stateParams, BoardServices) {


            $scope.organizationID = $stateParams.IdOrganization;
            $scope.NewBoardModel = { Title: '' };
           

            
            $scope.boards = [];


            $scope.getBoards = function() {
                BoardServices
                    .getBoardsForLoggedUser($scope.organizationID)
                    .success(function(data, status, headers, config) {

                        $scope.boards = data;
                    })
                    .error(function(data, status, headers, config) {
                        console.log(data);
                    });
                //$location.path('/');
            };

            $scope.CreateBoard = function() {
                $location.path('/loading');
                console.log($scope.NewBoardModel);
                BoardServices
                
                    .createBoardsForLoggedUser($scope.NewBoardModel, $scope.organizationID)
                    .success(function(data, status, headers, config) {
                        console.log($scope.NewBoardModel);
                        $location.path('/boards/' + $scope.organizationID);

                    })
                    .error(function(data, status, headers, config) {
                        console.log(data.Title);
                        console.log($scope.NewBoardModel);
                        //$location.path('/createboard/' + $scope.organizationID);
                    });
            };

            

            $scope.getBoards();


            $scope.$on('$viewContentLoaded', function() {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ])
    .controller('OrganizationController', [
        '$scope', '$location', '$window', 'OrganizationServices', '$stateParams', function($scope, $location, $window, organizationServices, $stateParams) {

            $scope.goToLoadingPage = function() {
                $location.path('/loading');
            };

            
            $scope.boardDetailId = $stateParams.boardId;

            //console.log($location.search().boardId);

            console.log($scope.boardDetailId);
            $scope.NewOrganizationModel = { Name: '', Description: '' };


            $scope.organizations = [];

            $scope.getOrganizationsForLoggedUser = function() {

                organizationServices
                    .getOrganizationsForLoggedUser()
                    .success(function(data, status, headers, config) {
                        $scope.organizations = data;
                        console.log(data);

                    })
                    .error(function(data, status, headers, config) {
                        console.log(data);
                    });

            };

            $scope.CreateOrganizationsForLoggedUser = function() {
                $scope.goToLoadingPage();

                organizationServices
                    .createOrganizationsForLoggedUser($scope.NewOrganizationModel)
                    .success(function(data, status, headers, config) {
                        console.log(data);
                        $location.path('/organizations');

                    })
                    .error(function(data, status, headers, config) {
                        console.log(data);
                        $location.path('/createorganization');
                    });

            };

            if ($scope.boardDetailId > 0) {
                //get board details
            } else {
                $scope.getOrganizationsForLoggedUser();
            }


            $scope.$on('$viewContentLoaded', function() {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ])
    .controller('LaneController', [
        '$scope', '$location', '$window', 'LaneServices', '$stateParams',  function($scope, $location, $window, LaneServices, $stateParams) {

            $scope.goToLoadingPage = function() {
                $location.path('/loading');
            };
            $scope.boardId = $stateParams.IdBoard;
            $scope.NewLaneModel = {Id: '', Name: ''};
            $scope.NewLaneName = { Name: ''};

            
            


            $scope.lanes = [];
            $scope.cards = [];
            
            /*var lane = { Name: 'Lane1' };
            var lane1 = { Name: 'Lane2' };
            var lane2 = { Name: 'Lane3' };
            var lane3 = { Name: 'Lane2' };
            var lane4 = { Name: 'Lane3' };
            $scope.lanes.push(lane);
            $scope.lanes.push(lane1);
            $scope.lanes.push(lane2);
            $scope.lanes.push(lane3);
            $scope.lanes.push(lane4);*/


            $scope.createLane = function () {
                $scope.goToLoadingPage();
                LaneServices.createLanesForLoggedUser($scope.NewLaneModel, $scope.boardId)
                .success(function (data, status, headers, config) {
                    
                    console.log(data);
                    $location.path('/lane/' + $scope.boardId);


                })
                    .error(function (data, status, headers, config) {
                        console.log(data);
                        $location.path('/lane/' + $scope.boardId);
                    });
            };
            $scope.getLanesForLoggedUser = function () {
                
                LaneServices
                    .getLanesForLoggedUser($scope.boardId)
                    .success(function (data, status, headers, config) {
                        $scope.lanes = data;
                        console.log(data);

                    })
                    .error(function (data, status, headers, config) {
                        console.log(data);
                    });

            };
           $scope.getCardsForLoggedUser = function(LaneId) {
                //$scope.goToLoadingPage();
                LaneServices
                .getCardsForLane(LaneId)
                .success(function (data) {
                    $scope.cards = data;
                    console.log(data);
                    $location.path('/lanes/' + $scope.boardId);


                })
                    .error(function (data, status, headers, config) {
                        console.log(data);
                        $location.path('/organizations');
                    });

            };

           /* if ($scope.lanes.length > 0)
            {
                $scope.getLanesForLoggedUser($scope.boardId);
            }
            else
            {
                var lane = { Name: 'Lane1' };
                var lane1 = { Name: 'Lane2' };
                var lane2 = { Name: 'Lane3' };
                var lane3 = { Name: 'Lane2' };
                var lane4 = { Name: 'Lane3' };
                $scope.lanes.push(lane);
                $scope.lanes.push(lane1);
                $scope.lanes.push(lane2);
                $scope.lanes.push(lane3);
                $scope.lanes.push(lane4);
            }*/

            $scope.getLanesForLoggedUser();

            
                
            


            $scope.$on('$viewContentLoaded', function() {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ])
    .controller('AccountController', [
        '$scope', '$location', '$window', 'AccountServices', '$stateParams', function($scope, $location, $window, AccountServices, $stateParams) {

            $scope.hasError = false;
            $scope.errorMessage = '';


            $scope.isLogged = function() {
                return $window.sessionStorage.token != null;
            };

            $scope.loginModel = { Email: '', Password: '' };
            $scope.changePasswordModel = { Email: '' };
            $scope.registerModel = { Email: '', Password: '', FirstName: '', LastName: '', ConfirmPassword: '' };
            $scope.AccountForgotPasswordModel = { Email: '', NewPassword: '', ConfirmNewPassword: '' };
            $scope.UserName = '';

            $scope.logout = function() {
                delete $window.sessionStorage.token;

            };

            $scope.login = function() {

                $scope.goToLoadingPage();
                console.log($scope.loginModel);

                AccountServices
                    .login($scope.loginModel)
                    .success(function(data, status, headers, config) {
                        $scope.goToLoadingPage();
                        $window.sessionStorage.token = data.Token;
                        $scope.UserName = data.Name;
                        console.log($scope.UserName);
                        $location.path('/organizations');

                    })
                    .error(function(data, status, headers, config) {
                        // Erase the token if the user fails to log in
                        delete $window.sessionStorage.token;
                        $scope.goToLogin();

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

            $scope.goToLoadingPage = function() {
                $location.path('/loading');
            };

            $scope.goToLogin = function() {
                $location.path('/login');
            };

            $scope.register = function() {
                $scope.goToLoadingPage();
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

            $scope.sendEmail = function() {
                $scope.goToLoadingPage();
                AccountServices.sendEmail($scope.changePasswordModel)
                    .success(function(data, status, headers, config) {
                        console.log(data);
                        $scope.goToLogin();

                    })
                    .error(function(data, status, headers, config) {
                        console.log(data);
                    });

            };

            $scope.forgotpassword = function() {

                $scope.goToLoadingPage();
                //$scope.sessionStorage.token = $stateParams.Token;
                //console.log($scope.sessionStorage.token);
                console.log($scope.AccountForgotPasswordModel);
                AccountServices.forgotPassword($stateParams.Token, $scope.AccountForgotPasswordModel)
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











