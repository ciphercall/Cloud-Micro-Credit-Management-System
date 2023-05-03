var Area = angular.module("Area", ['ui.bootstrap']);

Area.controller("ApiAreaController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {
        $scope.loading = true;
        event.preventDefault();

        // Grid view load 
        var cid = $('#txtCid').val();
        var uid = $('#txtUid').val();
        var tid = $('#txtTid').val();
        var branchid = $('#ddlBranchId option:selected').val();
        $http.get('/api/Area/List/', {
            params: {
                userCompanycode: cid,
                usercode: uid,
                branchId: branchid
            },
            headers: {
                'Authorization': 'token ' + tid,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            if (data.data.length === 0) {
                $scope.areaListData = null;
            } else {
                $scope.areaListData = data.data;
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
        var branchid1 = $('#ddlBranchId option:selected').val();
        $http.get('/api/Area/List/', {
            params: {
                userCompanycode: cid1,
                usercode: uid1,
                branchId: branchid1
            },
            headers: {
                'Authorization': 'token ' + tid1,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            $scope.areaData = data.data;
            $scope.loading = false;
        });
    };





    //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert === "I") {
            $('#txtAreaNameId').val("");
            $('#txtAuthorizedPersonId').val(""); 
            $('#ddlFieldWorkersId').val("0");
            alert("Permission Denied!!");
        } else {
            this.newChild.Compid = $('#txtCid').val();
            this.newChild.Userid = $('#txtUid').val();
            this.newChild.Insltude = $('#latlon').val();
            this.newChild.Branchid = $('#ddlBranchId option:selected').val();

            this.newChild.Areanm = $('#txtAreaNameId').val();
            this.newChild.Authpnm = $('#txtAuthorizedPersonId').val();
            this.newChild.Fwid = $('#ddlFieldWorkersId').val();
            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            if (this.newChild.Areanm !== "") {
                $http.post('/api/Area/Create?userCompanycode=' + this.newChild.Compid +
                                 "&usercode=" + this.newChild.Userid, this.newChild, config).success(function (data, status, headers, config) {

                                     //Grid view load 
                                     var cid2 = $('#txtCid').val();
                                     var uid2 = $('#txtUid').val();
                                     var tid2 = $('#txtTid').val();
                                     var branchid2 = $('#ddlBranchId option:selected').val();
                                     $http.get('/api/Area/List/', {
                                         params: {
                                             userCompanycode: cid2,
                                             usercode: uid2,
                                             branchId: branchid2
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid2,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.areaListData = data.data;
                                         $scope.loading = false;
                                     });

                                     $('#txtAreaNameId').val("");
                                     $('#txtAuthorizedPersonId').val("");
                                     $('#ddlFieldWorkersId').val("0");
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
        this.testitem.Branchid = $('#ddlBranchId option:selected').val();

        var update = $('#txt_Updateid').val();

        if (update === "I") {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var cid3 = $('#txtCid').val();
            var uid3 = $('#txtUid').val();
            var tid3 = $('#txtTid').val();
            var branchid3 = $('#ddlBranchId option:selected').val();
            $http.get('/api/Area/List/', {
                params: {
                    userCompanycode: cid3,
                    usercode: uid3,
                    branchId: branchid3
                },
                headers: {
                    'Authorization': 'token ' + tid3,
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            }).success(function (data) {
                $scope.areaListData = data.data;
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
            $http.post('/api/Area/Update?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     alert(data.message);
                                     frien.editMode = false;

                                     //Grid view load 
                                     var cid3 = $('#txtCid').val();
                                     var uid3 = $('#txtUid').val();
                                     var tid3 = $('#txtTid').val();
                                     var branchid3 = $('#ddlBranchId option:selected').val();
                                     $http.get('/api/Area/List/', {
                                         params: {
                                             userCompanycode: cid3,
                                             usercode: uid3,
                                             branchId: branchid3
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid3,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.areaListData = data.data;
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
        this.testitem.Branchid = $('#ddlBranchId option:selected').val();

        var dlete = $('#txt_Deleteid').val();
        if (dlete === "I") {
            alert("Permission Denied!!");
        } else {
            var compid = this.testitem.Compid;
            var branchid = this.testitem.Branchid;
            var areaid = this.testitem.Areaid;

            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            $http.post('/api/Area/Delete?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     if (data.success === true) {
                                         $.each($scope.areaListData, function (i) {
                                             if ($scope.areaListData[i].Compid === compid && $scope.areaListData[i].Branchid === branchid && $scope.areaListData[i].Areaid === areaid) {
                                                 $scope.areaListData.splice(i, 1);
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