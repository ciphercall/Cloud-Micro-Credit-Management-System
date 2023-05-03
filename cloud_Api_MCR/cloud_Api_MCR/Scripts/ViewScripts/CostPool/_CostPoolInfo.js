var CostPool = angular.module("CostPool", ['ui.bootstrap']);

CostPool.controller("ApiCostPoolController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {
        $scope.loading = true;
        event.preventDefault();

        // Grid view load 
        var cid = $('#txtCid').val();
        var uid = $('#txtUid').val();
        var tid = $('#txtTid').val();
        var costGroupid = $('#ddlgroupId option:selected').val();
        $http.get('/api/CostPool/List/', {
            params: {
                userCompanycode: cid,
                usercode: uid,
                costcid: costGroupid
            },
            headers: {
                'Authorization': 'token ' + tid,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            if (data.data.length === 0) {
                $scope.poolListData = null;
            } else {
                $scope.poolListData = data.data;
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
        var costGroupid1 = $('#ddlgroupId option:selected').val();
        $http.get('/api/CostPool/List/', {
            params: {
                userCompanycode: cid1,
                usercode: uid1,
                costcid: costGroupid1
            },
            headers: {
                'Authorization': 'token ' + tid1,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            $scope.poolListData = data.data;
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
            this.newChild.Costcid = $('#ddlgroupId option:selected').val();

            this.newChild.Costpnm = $('#txtHeadNameId').val();
            this.newChild.Costpsid = $('#txtCostPoolId').val();
            this.newChild.Remarks = $('#txtRemarksId').val();
            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            if (this.newChild.Costpnm !== "") {
                $http.post('/api/CostPool/Create?userCompanycode=' + this.newChild.Compid +
                                 "&usercode=" + this.newChild.Userid, this.newChild, config).success(function (data, status, headers, config) {

                                     //Grid view load 
                                     var cid2 = $('#txtCid').val();
                                     var uid2 = $('#txtUid').val();
                                     var tid2 = $('#txtTid').val();
                                     var costGroupid2 = $('#ddlgroupId option:selected').val();
                                     $http.get('/api/CostPool/List/', {
                                         params: {
                                             userCompanycode: cid2,
                                             usercode: uid2,
                                             costcid: costGroupid2
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid2,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.poolListData = data.data;
                                         $scope.loading = false;
                                     });

                                     $('#txtHeadNameId').val(""); 
                                     $('#txtCostPoolId').val("");
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
        this.testitem.Costcid = $('#ddlgroupId option:selected').val();

        var update = $('#txt_Updateid').val();

        if (update === "I") {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var cid3 = $('#txtCid').val();
            var uid3 = $('#txtUid').val();
            var tid3 = $('#txtTid').val();
            var costGroupid3 = $('#ddlgroupId option:selected').val();
            $http.get('/api/CostPool/List/', {
                params: {
                    userCompanycode: cid3,
                    usercode: uid3,
                    costcid: costGroupid3
                },
                headers: {
                    'Authorization': 'token ' + tid3,
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            }).success(function (data) {
                $scope.poolListData = data.data;
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
            $http.post('/api/CostPool/Update?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     alert(data.message);
                                     frien.editMode = false;

                                     //Grid view load 
                                     var cid3 = $('#txtCid').val();
                                     var uid3 = $('#txtUid').val();
                                     var tid3 = $('#txtTid').val();
                                     var costGroupid3 = $('#ddlgroupId option:selected').val();
                                     $http.get('/api/CostPool/List/', {
                                         params: {
                                             userCompanycode: cid3,
                                             usercode: uid3,
                                             costcid: costGroupid3
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid3,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.poolListData = data.data;
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
        this.testitem.Costcid = $('#ddlgroupId option:selected').val();

        var dlete = $('#txt_Deleteid').val();
        if (dlete === "I") {
            alert("Permission Denied!!");
        } else {
            var compid = this.testitem.Compid;
            var costcid = this.testitem.Costcid;
            var costpid = this.testitem.Costpid;

            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            $http.post('/api/CostPool/Delete?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     if (data.success === true) {
                                         $.each($scope.poolListData, function (i) {
                                             if ($scope.poolListData[i].Compid === compid && $scope.poolListData[i].Costcid === costcid && $scope.poolListData[i].Costpid === costpid) {
                                                 $scope.poolListData.splice(i, 1);
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