var CostPoolMaster = angular.module("CostPoolMaster", ['ui.bootstrap']);

CostPoolMaster.controller("ApiCostPoolMasterController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    // Grid view load 
    var cid = $('#txtCid').val();
    var uid = $('#txtUid').val();
    var tid = $('#txtTid').val();
    $http.get('/api/CostPoolMaster/List/', {
        params: {
            userCompanycode: cid,
            usercode: uid
        },
        headers: {
            'Authorization': 'token ' + tid,
            'Accept': 'application/json',
            "X-Login-Ajax-call": 'true'
        }
    }).success(function (data) {
        if (data.data.length === 0) {
            $scope.poolMasterData = null;
        } else {
            $scope.poolMasterData = data.data;
        }
        $scope.loading = false;
    });






    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };



    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;

        //Grid view load 
        var cid1 = $('#txtCid').val();
        var uid1 = $('#txtUid').val();
        var tid1 = $('#txtTid').val();
        $http.get('/api/CostPoolMaster/List/', {
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
            $scope.poolMasterData = data.data;
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
            alert("Permission Denied!!");
        } else {
            this.newChild.Compid = $('#txtCid').val();
            this.newChild.Userid = $('#txtUid').val();
            this.newChild.Insltude = $('#latlon').val();

            this.newChild.Costcnm = $('#txtHeadNameId').val();
            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            if (this.newChild.Costcnm !== "") {
                $http.post('/api/CostPoolMaster/Create?userCompanycode=' + this.newChild.Compid +
                                 "&usercode=" + this.newChild.Userid, this.newChild, config).success(function (data, status, headers, config) {

                                     //Grid view load 
                                     var cid2 = $('#txtCid').val();
                                     var uid2 = $('#txtUid').val();
                                     var tid2 = $('#txtTid').val();
                                     $http.get('/api/CostPoolMaster/List/', {
                                         params: {
                                             userCompanycode: cid2,
                                             usercode: uid2
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid2,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.poolMasterData = data.data;
                                         $scope.loading = false;
                                     });

                                     $('#txtHeadNameId').val("");
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


        var update = $('#txt_Updateid').val();

        if (update === "I" || this.testitem.Costcnm === "" || this.testitem.Costcnm === null) {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var cid3 = $('#txtCid').val();
            var uid3 = $('#txtUid').val();
            var tid3 = $('#txtTid').val();
            $http.get('/api/CostPoolMaster/List/', {
                params: {
                    userCompanycode: cid3,
                    usercode: uid3
                },
                headers: {
                    'Authorization': 'token ' + tid3,
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            }).success(function (data) {
                $scope.poolMasterData = data.data;
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
            $http.post('/api/CostPoolMaster/Update?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     alert(data.message);
                                     frien.editMode = false;

                                     //Grid view load 
                                     var cid3 = $('#txtCid').val();
                                     var uid3 = $('#txtUid').val();
                                     var tid3 = $('#txtTid').val()
                                     $http.get('/api/CostPoolMaster/List/', {
                                         params: {
                                             userCompanycode: cid3,
                                             usercode: uid3
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid3,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.poolMasterData = data.data;
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

        var dlete = $('#txt_Deleteid').val();
        if (dlete === "I") {
            alert("Permission Denied!!");
        } else {
            var compid = this.testitem.Compid;
            var costcid = this.testitem.Costcid;

            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            $http.post('/api/CostPoolMaster/Delete?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     if (data.success === true) {
                                         $.each($scope.poolMasterData, function (i) {
                                             if ($scope.poolMasterData[i].Compid === compid && $scope.poolMasterData[i].Costcid === costcid) {
                                                 $scope.poolMasterData.splice(i, 1);
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