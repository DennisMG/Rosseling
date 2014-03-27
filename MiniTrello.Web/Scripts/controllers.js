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

            $scope.DeleteBoard = function (idBoard) {
                //$scope.OrganizationArchiveModel.Id = idOrganization;
                console.log($scope.OrganizationArchiveModel);
                BoardServices
                    .deleteBoard(idBoard)
                    .success(function (data) {
                        console.log(data);
                        $scope.getBoards();
                        //$scope.organizations.pop(data);


                    })
                    .error(function (data, status, headers, config) {
                        console.log(data);

                    });

            };

            $scope.CreateBoard = function() {
               // $location.path('/loading');
                console.log($scope.NewBoardModel);
                BoardServices
                
                    .createBoardsForLoggedUser($scope.NewBoardModel, $scope.organizationID)
                    .success(function(data, status, headers, config) {
                        console.log($scope.NewBoardModel);
                        $scope.boards.push(data);
                        //$scope.getBoards();
                        //$location.path('/boards/' + $scope.organizationID);

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
        '$scope', '$location', '$window', 'OrganizationServices', '$stateParams', function($scope, $location, $window, OrganizationServices, $stateParams) {

            $scope.goToLoadingPage = function() {
                $location.path('/loading');
            };

            
            $scope.boardDetailId = $stateParams.boardId;
            //console.log($scope.boardDetailId);
            $scope.NewOrganizationModel = { Name: '', Description: '' };
            $scope.OrganizationArchiveModel = { Id: 0 };

            $scope.organizations = [];

            $scope.getOrganizationsForLoggedUser = function() {

                OrganizationServices
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
               

                OrganizationServices
                    .createOrganizationsForLoggedUser($scope.NewOrganizationModel)
                    .success(function (data, status, headers, config) {
                        
                        console.log(data);
                        $scope.organizations.push(data);
                        //$scope.getOrganizationsForLoggedUser();
                       
                        

                    })
                    .error(function(data, status, headers, config) {
                        console.log(data);
                        
                    });

            };
            

            

            $scope.DeleteOrganization = function (idOrganization) {
                //$scope.OrganizationArchiveModel.Id = idOrganization;
                console.log($scope.OrganizationArchiveModel);
                OrganizationServices
                    .deleteOrganization(idOrganization)
                    .success(function (data) {
                        console.log(data);
                        $scope.getOrganizationsForLoggedUser();
                        //$scope.organizations.pop(data);


                    })
                    .error(function (data, status, headers, config) {
                        console.log(data);
                        
                    });
                
            };

            
            $scope.getOrganizationsForLoggedUser();


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
            //$scope.cards = [];
            
            $scope.createLane = function () {
                //$scope.goToLoadingPage();
                LaneServices.createLanesForLoggedUser($scope.NewLaneModel, $scope.boardId)
                .success(function (data, status, headers, config) {
                        $scope.lanes.push(data);
                        toastr.success("", "New Lane Created");


                })
                    .error(function (data, status, headers, config) {
                        toastr.error("Failed to create Lane", "Error");
                        $location.path('/lane/' + $scope.boardId);
                    });
            };
            $scope.getLanesForLoggedUser = function () {
                
                LaneServices
                    .getLanesForLoggedUser($scope.boardId)
                    .success(function (data, status, headers, config) {
                        $scope.lanes = data;
                        

                    })
                    .error(function (data, status, headers, config) {
                        console.log(data);
                    });

            };

            $scope.DeleteLane = function (idLane) {
                LaneServices
                    .deleteLane(idLane)
                    .success(function (data) {
                        
                        $scope.getLanesForLoggedUser();
                        toastr.success("", "Lane Deleted");

                    })
                    .error(function (data, status, headers, config) {
                        console.log(data);
                        toastr.error("Failed deleting Lane", "Error");

                    });

            };

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
            $scope.UpdateAccountModel = {FirstName:'',LastName:'',Email:''};

            $scope.isLogged = function() {
                return $window.sessionStorage.token != null;
            };

            $scope.loginModel = { Email: '', Password: '' };
            $scope.changePasswordModel = { Email: '' };
            $scope.registerModel = { Email: '', Password: '', FirstName: '', LastName: '', ConfirmPassword: '' };
            $scope.AccountForgotPasswordModel = { Email: '', NewPassword: '', ConfirmNewPassword: '' };
            $scope.UserName = '';

            $scope.UpdateAccount = function() {
                AccountServices.updateAccount($scope.UpdateAccountModel)
                .success(function(data) {
                        
                        toastr.success("", "Account Updated");
                    })
                    .error(function (data) {
                        toastr.error("Failed Updating account data", "Error");

                    console.log(data);

                });
            };

            $scope.GetAccount = function () {
                AccountServices
                    .getAccount()
                    .success(function (data) {
                        $scope.UpdateAccountModel = data;

                    })
                    .error(function (data) {
                        console.log(data);
                        toastr.error("", "Failed to retrieve data for this user.");

                    });
            };

            $scope.logout = function() {
                delete $window.sessionStorage.token;

            };

            $scope.login = function() {

                AccountServices
                    .login($scope.loginModel)
                    .success(function (data, status, headers, config) {
                       
                        toastr.success("", "Bienvenido a MiniTrello",
                                {
                                    "positionClass": "toast-top-full-width",
                                    "showEasing": "swing",
                                    "hideEasing": "swing",
                                    "timeOut": "1000",
                                    "showMethod": "slideDown",
                                    "hideMethod": "fadeOut"
                                });
                        
                        $window.sessionStorage.token = data.Token;
                        $scope.UserName = data.Name;
                        console.log($scope.UserName);
                        $location.path('/organizations');
                        

                    })
                    .error(function (data, status, headers, config) {
                        toastr.error("Your Email or Password may be incorrect", "Oops");
                    
                        delete $window.sessionStorage.token;
                        $scope.goToLogin();

                        $scope.errorMessage = 'Error o clave incorrect';
                        $scope.hasError = true;
                       
                        $scope.message = 'Error: Invalid user or password';
                    });
                
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
               
                console.log($scope.registerModel);
                AccountServices
                    .register($scope.registerModel)
                    .success(function(data, status, headers, config) {
                        toastr.success("Now you are part of the MiniTrello Community", "Congratulations");
                        $scope.goToLogin();

                    })
                    .error(function(data, status, headers, config) {
                        
                        toastr.error("Something went terribly wrong. We couldn't create your new account :S Please check your Password and Password confirmation and try again", "Oops",
                            {
                                
                                "showEasing": "swing",
                                "hideEasing": "swing",
                                "timeOut": "5000",
                                "showMethod": "slideDown",
                                "hideMethod": "fadeOut"
                            });
                        
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

            $scope.GetAccount();


            $scope.$on('$viewContentLoaded', function() {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            });
        }
    ]);











