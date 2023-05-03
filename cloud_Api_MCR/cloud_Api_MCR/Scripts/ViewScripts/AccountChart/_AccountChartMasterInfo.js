﻿var AccountChartMaster = angular.module("AccountChartMaster", ['ui.bootstrap']);

AccountChartMaster.controller("ApiAccountChartMasterController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {
        $scope.loading = true;
        event.preventDefault();

        // Grid view load 
        var cid = $('#txtCid').val();
        var uid = $('#txtUid').val();
        var tid = $('#txtTid').val();
        var hdTypeid = $('#ddlHeadTypeId option:selected').val();
        $http.get('/api/AccountChartMaster/List/', {
            params: {
                userCompanycode: cid,
                usercode: uid,
                headType: hdTypeid
            },
            headers: {
                'Authorization': 'token ' + tid,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            if (data.data.length === 0) {
                $scope.chartMasterData = null;
            } else {
                $scope.chartMasterData = data.data;
            }
            $scope.loading = false;
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
        var hdTypeid1 = $('#ddlHeadTypeId option:selected').val();
        $http.get('/api/AccountChartMaster/List/', {
            params: {
                userCompanycode: cid1,
                usercode: uid1,
                headType: hdTypeid1
            },
            headers: {
                'Authorization': 'token ' + tid1,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            $scope.chartMasterData = data.data;
            $scope.loading = false;
        });
    };





    //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert === "I") {
            $('#txtHeadNameId').val("");
            $('#txtRemarksId').val("");
            alert("Permission Denied!!");
        } else {
            this.newChild.Compid = $('#txtCid').val();
            this.newChild.Userid = $('#txtUid').val();
            this.newChild.Insltude = $('#latlon').val();
            this.newChild.Headtp = $('#ddlHeadTypeId option:selected').val();

            this.newChild.Headnm = $('#txtHeadNameId').val();
            this.newChild.Remarks = $('#txtRemarksId').val();
            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            if (this.newChild.Headnm !== "") {
                $http.post('/api/AccountChartMaster/Create?userCompanycode=' + this.newChild.Compid +
                                 "&usercode=" + this.newChild.Userid, this.newChild, config).success(function (data, status, headers, config) {

                                     //Grid view load 
                                     var cid2 = $('#txtCid').val();
                                     var uid2 = $('#txtUid').val();
                                     var tid2 = $('#txtTid').val();
                                     var hdTypeid2 = $('#ddlHeadTypeId option:selected').val();
                                     $http.get('/api/AccountChartMaster/List/', {
                                         params: {
                                             userCompanycode: cid2,
                                             usercode: uid2,
                                             headType: hdTypeid2
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid2,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.chartMasterData = data.data;
                                         $scope.loading = false;
                                     });

                                     $('#txtHeadNameId').val("");
                                     $('#txtRemarksId').val("");
                                     alert(data.message);
                                 }).error(function () {
                                     $scope.error = "An Error has occured while loading posts!";
                                     $scope.loading = false;
                                 });
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
        this.testitem.Headtp = $('#ddlHeadTypeId option:selected').val();


        var update = $('#txt_Updateid').val();

        if (update === "I" || this.testitem.Headnm === "" || this.testitem.Headnm === null) {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var cid3 = $('#txtCid').val();
            var uid3 = $('#txtUid').val();
            var tid3 = $('#txtTid').val();
            var hdTypeid3 = $('#ddlHeadTypeId option:selected').val();
            $http.get('/api/AccountChartMaster/List/', {
                params: {
                    userCompanycode: cid3,
                    usercode: uid3,
                    headType: hdTypeid3
                },
                headers: {
                    'Authorization': 'token ' + tid3,
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            }).success(function (data) {
                $scope.chartMasterData = data.data;
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
            $http.post('/api/AccountChartMaster/Update?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     alert(data.message);
                                     frien.editMode = false;

                                     //Grid view load 
                                     var cid3 = $('#txtCid').val();
                                     var uid3 = $('#txtUid').val();
                                     var tid3 = $('#txtTid').val();
                                     var hdTypeid3 = $('#ddlHeadTypeId option:selected').val();
                                     $http.get('/api/AccountChartMaster/List/', {
                                         params: {
                                             userCompanycode: cid3,
                                             usercode: uid3,
                                             headType: hdTypeid3
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid3,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.chartMasterData = data.data;
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
        this.testitem.Headtp = $('#ddlHeadTypeId option:selected').val();

        var dlete = $('#txt_Deleteid').val();
        if (dlete == "I") {
            alert("Permission Denied!!");
        } else {
            var compid = this.testitem.Compid;
            var headcd = this.testitem.Headcd;
            var headtp = this.testitem.Headtp;

            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            $http.post('/api/AccountChartMaster/Delete?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     if (data.success === true) {
                                         $.each($scope.chartMasterData, function (i) {
                                             if ($scope.chartMasterData[i].Compid === compid && $scope.chartMasterData[i].Headcd === headcd && $scope.chartMasterData[i].Headtp === headtp) {
                                                 $scope.chartMasterData.splice(i, 1);
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


});