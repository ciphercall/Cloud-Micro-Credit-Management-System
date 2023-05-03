var ModuleApp = angular.module("ModuleApp", ['ui.bootstrap']);

ModuleApp.controller("ApiModuleController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    // Inital Grid view load 
    var cid = $('#txtCid').val();
    var uid = $('#txtUid').val();
    var tid = $('#txtTid').val();
    $http.get('/api/Module/ModuleList/', {
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
            $scope.ModuleData = null;
        } else {
            $scope.ModuleData = data.data;
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
        $http.get('/api/Module/ModuleList/', {
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
            $scope.ModuleData = data.data;
            $scope.loading = false;
        });
    };





    //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        this.newChild.Compid = $('#txtCid').val();
        this.newChild.Userid = $('#txtUid').val();
        this.newChild.Insltude = $('#latlon').val();
        this.newChild.Modulenm = $('#txtModuleNameId').val();
        var config = {
            headers: {
                'Authorization': 'token ' + $('#txtTid').val(),
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        };
        if (this.newChild.Modulenm !== "") {
            $http.post('/api/Module/Create', this.newChild, config).success(function (data, status, headers, config) {

                //Grid view load 
                var cid2 = $('#txtCid').val();
                var uid2 = $('#txtUid').val();
                var tid2 = $('#txtTid').val();
                $http.get('/api/Module/ModuleList/', {
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
                    $scope.ModuleData = data.data;
                    $scope.loading = false;
                });

                $('#txtModuleNameId').val("");
                alert(data.message);
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });
        }
    };







    //Update to grid level data (save a record after edit)
    $scope.update = function () {
        $scope.loading = true;
        var frien = this.testitem;

        this.testitem.Compid = $('#txtCid').val();
        this.testitem.Userid = $('#txtUid').val();
        this.testitem.Insltude = $('#latlon').val();
        var config = {
            headers: {
                'Authorization': 'token ' + $('#txtTid').val(),
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        };
        //this.testitem.Modulenm = $('#txtModuleNameId').val();
        $http.post('/api/Module/Update', this.testitem,config).success(function (data) {
            alert(data.message);
            frien.editMode = false;

            //Grid view load 
            var cid2 = $('#txtCid').val();
            var uid2 = $('#txtUid').val();
            var tid2 = $('#txtTid').val();
            $http.get('/api/Module/ModuleList/', {
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
                $scope.ModuleData = data.data;
            });
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;
        });
    };







    //Delete grid level data.
    $scope.deleteItem = function () {
        $scope.loading = true;
        var id = this.testitem.Moduleid;

        this.testitem.Compid = $('#txtCid').val();
        this.testitem.Userid = $('#txtUid').val();
        this.testitem.Insltude = $('#latlon').val();
        var config = {
            headers: {
                'Authorization': 'token ' + $('#txtTid').val(),
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        };
        $http.post('/api/Module/Delete', this.testitem, config).success(function (data) {

            $.each($scope.ModuleData, function (i) {
                if ($scope.ModuleData[i].Moduleid === id) {
                    $scope.ModuleData.splice(i, 1);
                    return false;
                }
            });
            $scope.loading = false;
            alert(data.message);
        }).error(function (data) {
            $scope.error = "An Error has occured while delete posts! " + data;
            $scope.loading = false;
        });

    };


});