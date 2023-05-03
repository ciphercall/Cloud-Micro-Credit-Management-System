var MenuApp = angular.module("MenuApp", ['ui.bootstrap']);

MenuApp.controller("ApiMenuController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {
        $scope.loading = true;
        event.preventDefault();
        
        // Grid view load 
        var cid = $('#txtCid').val();
        var uid = $('#txtUid').val();
        var tid = $('#txtTid').val();
        var moduleid = $('#ddlModuleId option:selected').val();
        var menutypeid = $('#ddlMenuTypeId option:selected').val();
        $http.get('/api/Menu/MenuList/', {
            params: {
                userCompanycode: cid,
                usercode: uid,
                moduleId: moduleid,
                menuType: menutypeid
            },
            headers: {
                'Authorization': 'token ' + tid,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            if (data.data.length === 0) {
                $scope.MenuData = null;
            } else {
                $scope.MenuData = data.data;
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
        var moduleid1 = $('#ddlModuleId option:selected').val();
        var menutypeid1 = $('#ddlMenuTypeId option:selected').val();
        $http.get('/api/Menu/MenuList/', {
            params: {
                userCompanycode: cid1,
                usercode: uid1,
                moduleId: moduleid1,
                menuType: menutypeid1
            },
            headers: {
                'Authorization': 'token ' + tid1,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            $scope.MenuData = data.data;
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
        this.newChild.Moduleid = $('#ddlModuleId option:selected').val();
        this.newChild.Menutp = $('#ddlMenuTypeId option:selected').val();

        this.newChild.Serial = $('#txtSerialId').val();
        this.newChild.Menunm = $('#txtMenuNameId').val();
        this.newChild.Actionname = $('#txtActionNameId').val();
        this.newChild.Controllername = $('#txtControllerNameId').val();
        var config = {
            headers: {
                'Authorization': 'token ' + $('#txtTid').val(),
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        };
        if (this.newChild.Modulenm !== "") {
            $http.post('/api/Menu/Create', this.newChild, config).success(function (data, status, headers, config) {

                //Grid view load 
                var cid2 = $('#txtCid').val();
                var uid2 = $('#txtUid').val();
                var tid2 = $('#txtTid').val();
                var moduleid2 = $('#ddlModuleId option:selected').val();
                var menutypeid2 = $('#ddlMenuTypeId option:selected').val();
                $http.get('/api/Menu/MenuList/', {
                    params: {
                        userCompanycode: cid2,
                        usercode: uid2,
                        moduleId: moduleid2,
                        menuType: menutypeid2
                    },
                    headers: {
                        'Authorization': 'token ' + tid2,
                        'Accept': 'application/json',
                        "X-Login-Ajax-call": 'true'
                    }
                }).success(function (data) {
                    $scope.MenuData = data.data;
                    $scope.loading = false;
                });

                $('#txtSerialId').val("");
                $('#txtMenuNameId').val("");
                $('#txtActionNameId').val("");
                $('#txtControllerNameId').val("");
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
        this.testitem.Moduleid = $('#ddlModuleId option:selected').val();
        this.testitem.Menutp = $('#ddlMenuTypeId option:selected').val();
        var config = {
            headers: {
                'Authorization': 'token ' + $('#txtTid').val(),
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        };
        $http.post('/api/Menu/Update', this.testitem, config).success(function (data) {
            alert(data.message);
            frien.editMode = false;

            //Grid view load 
            var cid3 = $('#txtCid').val();
            var uid3 = $('#txtUid').val();
            var tid3= $('#txtTid').val();
            var moduleid3 = $('#ddlModuleId option:selected').val();
            var menutypeid3 = $('#ddlMenuTypeId option:selected').val();
            $http.get('/api/Menu/MenuList/', {
                params: {
                    userCompanycode: cid3,
                    usercode: uid3,
                    moduleId: moduleid3,
                    menuType: menutypeid3
                },
                headers: {
                    'Authorization': 'token ' + tid3,
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            }).success(function (data) {
                $scope.MenuData = data.data;
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
        var moduleid = this.testitem.Moduleid;
        var menutypeid = this.testitem.Menutp;
        var menuid = this.testitem.Menuid;

        this.testitem.Compid = $('#txtCid').val();
        this.testitem.Userid = $('#txtUid').val();
        this.testitem.Insltude = $('#latlon').val();
        this.testitem.Moduleid = $('#ddlModuleId option:selected').val();
        this.testitem.Menutp = $('#ddlMenuTypeId option:selected').val();
        var config = {
            headers: {
                'Authorization': 'token ' + $('#txtTid').val(),
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        };
        $http.post('/api/Menu/Delete', this.testitem, config).success(function (data) {

            $.each($scope.MenuData, function (i) {
                if ($scope.MenuData[i].Moduleid === moduleid && $scope.MenuData[i].Menutp === menutypeid && $scope.MenuData[i].Menuid === menuid) {
                    $scope.MenuData.splice(i, 1);
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