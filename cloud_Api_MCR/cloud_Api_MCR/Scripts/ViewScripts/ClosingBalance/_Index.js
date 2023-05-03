var ClosingBalanceApp = angular.module("ClosingBalanceApp", ['ui.bootstrap']);

ClosingBalanceApp.controller("ApiClosingBalanceController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {
        $scope.loading = true;
        event.preventDefault();

        // Grid view load 
        var cid = $('#txtCid').val();
        var uid = $('#txtUid').val();
        var tid = $('#txtTid').val();
        var date = $('#txtDate').val();

       
        $http.get('/api/ClosingBalance/List/', {
            params: {
                userCompanycode: cid,
                usercode: uid,
                transDate: date
            },
            headers: {
                'Authorization': 'token ' + tid,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            if (data.data.length === 0) {
                dateFind();
                $scope.closingBalanceData = null;
                $('#List').hide();
                
            } else {
                $('#List').show();
                alert("Continue");
                $scope.closingBalanceData = data.data;
            }
            $scope.loading = false;
        });

    };

    
    
    function dateFind() {
        //Get Opening Date
        var cid1 = $('#txtCid').val();
        var uid1 = $('#txtUid').val();
        var tid1 = $('#txtTid').val();
        $http.get('/api/ClosingBalance/GetOpeningDate/', {
            params: {
                userCompanycode: cid1,
                usercode: uid1
            },
            headers: {
                'Authorization': 'token ' + tid1,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            var getDate = data.data;
            $scope.loading = false;
            alert("Closing Balance has already entered, DATE: " + getDate);
        });
    };






    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };



    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;

        //Grid view load 
        var cid1 = $('#txtCid').val();
        var uid1 = $('#txtUid').val();
        var tid1 = $('#txtTid').val();
        var date1 = $('#txtDate').val();
        $http.get('/api/ClosingBalance/List/', {
            params: {
                userCompanycode: cid1,
                usercode: uid1,
                transDate: date1
            },
            headers: {
                'Authorization': 'token ' + tid1,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            $scope.closingBalanceData = data.data;
            $scope.loading = false;
        });
    };





    //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert === "I") {
            $('#txtAccountNameId').val("");
            $('#txtRemarksId').val("");
            alert("Permission Denied!!");
        } else {
            this.newChild.Compid = $('#txtCid').val();
            this.newChild.Userid = $('#txtUid').val();
            this.newChild.Insltude = $('#latlon').val();
            this.newChild.Transdt = $('#txtDate').val();

            this.newChild.Debitcd = $('#txtDebitId').val();
            this.newChild.Debitamt = $('#txtDebitAmountId').val();
            this.newChild.Creditamt = $('#txtCreditAmountId').val();
            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            if (this.newChild.Debitcd !== "" && this.newChild.Debitcd !== null && this.newChild.Debitcd !== 0 && this.newChild.Debitamt !== 0 && this.newChild.Creditamt !== 0) {
                $http.post('/api/ClosingBalance/Create?userCompanycode=' + this.newChild.Compid +
                    "&usercode=" + this.newChild.Userid, this.newChild, config).success(function(data, status, headers, config) {

                    //Grid view load 
                    var cid2 = $('#txtCid').val();
                    var uid2 = $('#txtUid').val();
                    var tid2 = $('#txtTid').val();
                    var date2 = $('#txtDate').val();
                    $http.get('/api/ClosingBalance/List/', {
                        params: {
                            userCompanycode: cid2,
                            usercode: uid2,
                            transDate: date2
                        },
                        headers: {
                            'Authorization': 'token ' + tid2,
                            'Accept': 'application/json',
                            "X-Login-Ajax-call": 'true'
                        }
                    }).success(function(data) {
                        $scope.closingBalanceData = data.data;
                        $scope.loading = false;
                    });

                    $('#txtDebitId').select2.defaults.set("ajax--cache", false);
                    $('#txtDebitAmountId').val("0");
                    $('#txtCreditAmountId').val("0");
                    alert(data.message);
                }).error(function() {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });
            } else {
                $('#txtDebitId').select2.defaults.set("ajax--cache", false);
                $('#txtDebitAmountId').val("0");
                $('#txtCreditAmountId').val("0");
                alert("Validation Error!!!");
            }
        }

    };







    //Update to grid level data (save a record after edit)
    $scope.update = function () {
        $scope.loading = true;
        var frien = this.testitem;

        this.testitem.Compid = $('#txtCid').val();
        this.testitem.Userid = $('#txtUid').val();
        this.testitem.Updltude = $('#latlon').val();
        this.testitem.Transdt = $('#txtDate').val();

        var update = $('#txt_Updateid').val();

        if (update === "I") {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var cid3 = $('#txtCid').val();
            var uid3 = $('#txtUid').val();
            var tid3 = $('#txtTid').val();
            var date3 = $('#txtDate').val();
            $http.get('/api/ClosingBalance/List/', {
                params: {
                    userCompanycode: cid3,
                    usercode: uid3,
                    transDate: date3
                },
                headers: {
                    'Authorization': 'token ' + tid3,
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            }).success(function (data) {
                $scope.closingBalanceData = data.data;
            });
            $scope.loading = false;
        }
        else {

            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            $http.post('/api/ClosingBalance/Update?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     alert(data.message);
                                     frien.editMode = false;

                                     //Grid view load 
                                     var cid3 = $('#txtCid').val();
                                     var uid3 = $('#txtUid').val();
                                     var tid3 = $('#txtTid').val();
                                     var date3 = $('#txtDate').val();
                                     $http.get('/api/ClosingBalance/List/', {
                                         params: {
                                             userCompanycode: cid3,
                                             usercode: uid3,
                                             transDate: date3
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid3,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.closingBalanceData = data.data;
                                     });
                                     $scope.loading = false;
                                 }).error(function (data) {
                                     $scope.error = "An Error has occured while Saving Friend! " + data;
                                     $scope.loading = false;
                                 });

        }



    };







    //Delete grid level data.
    $scope.deleteItem = function () {
        $scope.loading = true;

        this.testitem.Compid = $('#txtCid').val();
        this.testitem.Userid = $('#txtUid').val();
        this.testitem.Updltude = $('#latlon').val();
        this.testitem.Transdt = $('#txtDate').val();

        var dlete = $('#txt_Deleteid').val();
        if (dlete === "I") {
            alert("Permission Denied!!");
        } else {
            var compid = this.testitem.Compid;
            var transtp = this.testitem.Transtp;
            var transmy = this.testitem.Transmy;
            var transno = this.testitem.Transno;
            var transsl = this.testitem.Transsl;

            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            $http.post('/api/ClosingBalance/Delete?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     if (data.success === true) {
                                         $.each($scope.closingBalanceData, function (i) {
                                             if ($scope.closingBalanceData[i].Compid === compid && $scope.closingBalanceData[i].Transtp === transtp && $scope.closingBalanceData[i].Transmy === transmy && $scope.closingBalanceData[i].Transno === transno && $scope.closingBalanceData[i].Transsl === transsl) {
                                                 $scope.closingBalanceData.splice(i, 1);
                                                 return false;
                                             }
                                         });
                                         $scope.loading = false;
                                     }
                                     alert(data.message);
                                 }).error(function (data) {
                                     $scope.error = "An Error has occured while delete posts! " + data;
                                     $scope.loading = false;
                                 });
        }
    };





    $scope.GetSummOfDebitAmount = function (closingBalanceData) {
        var summ = 0;
        for (var i in closingBalanceData) {
            if (closingBalanceData.hasOwnProperty(i)) {
                summ = summ + Number(closingBalanceData[i].Debitamt);
            }
        }
        $('#gridTotalDebitAmount').val(summ);
        return summ;
    };


    $scope.GetSummOfCreditAmount = function (closingBalanceData) {
        var summ = 0;
        for (var i in closingBalanceData) {
            if (closingBalanceData.hasOwnProperty(i)) {
                summ = summ + Number(closingBalanceData[i].Creditamt);
            }
        }
        $('#gridTotalCreditAmount').val(summ);
        return summ;
    };


});