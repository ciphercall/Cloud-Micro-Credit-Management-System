var MenuPermissionApp = angular.module("MenuPermissionApp", ['ui.bootstrap']);

MenuPermissionApp.controller("ApiRoleController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {
        $scope.loading = true;
        event.preventDefault();

        // Grid view load 
        var cid = $('#txtCid').val();
        var uid = $('#txtUid').val();
        var tid = $('#txtTid').val();
        var selectCompanyId = $('#ddlCompanyId option:selected').val();
        var moduleid = $('#ddlModuleId option:selected').val();
        var menutypeid = $('#ddlMenuTypeId option:selected').val();
        $http.get('/api/Role/CompanyWisePermissionList/', {
            params: {
                userCompanycode: cid,
                usercode: uid,
                selectCompanyCode: selectCompanyId,
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
                $scope.RoleData = null;
            } else {
                $scope.RoleData = data.data;
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
        var selectCompanyId1 = $('#ddlCompanyId option:selected').val();
        var moduleid1 = $('#ddlModuleId option:selected').val();
        var menutypeid1 = $('#ddlMenuTypeId option:selected').val();
        $http.get('/api/Role/CompanyWisePermissionList/', {
            params: {
                userCompanycode: cid1,
                usercode: uid1,
                selectCompanyCode: selectCompanyId1,
                moduleId: moduleid1,
                menuType: menutypeid1
            },
            headers: {
                'Authorization': 'token ' + tid1,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            $scope.RoleData = data.data;
            $scope.loading = false;
        });
    };





    //Update to grid level data (save a record after edit)
    $scope.update = function () {
        $scope.loading = true;
        var frien = this.testitem;

        this.testitem.Compid = $('#txtCid').val();
        this.testitem.Userid = $('#txtUid').val();
        this.testitem.Insltude = $('#latlon').val();
        this.testitem.SelectCompId = $('#ddlCompanyId option:selected').val();
        this.testitem.Moduleid = $('#ddlModuleId option:selected').val();
        this.testitem.Menutp = $('#ddlMenuTypeId option:selected').val();
        var config = {
            headers: {
                'Authorization': 'token ' + $('#txtTid').val(),
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        };
        if (this.testitem.SelectCompId !== "" && this.testitem.Moduleid !== "" && this.testitem.Menutp !== "") {
            $http.post('/api/Role/CompanyWisePermissionUpdate', this.testitem, config).success(function (data) {
                alert(data.message);
                frien.editMode = false;

                //Grid view load 
                var cid3 = $('#txtCid').val();
                var uid3 = $('#txtUid').val();
                var tid3 = $('#txtTid').val();
                var selectCompanyId3 = $('#ddlCompanyId option:selected').val();
                var moduleid3 = $('#ddlModuleId option:selected').val();
                var menutypeid3 = $('#ddlMenuTypeId option:selected').val();
                $http.get('/api/Role/CompanyWisePermissionList/', {
                    params: {
                        userCompanycode: cid3,
                        usercode: uid3,
                        selectCompanyCode: selectCompanyId3,
                        moduleId: moduleid3,
                        menuType: menutypeid3
                    },
                    headers: {
                        'Authorization': 'token ' + tid3,
                        'Accept': 'application/json',
                        "X-Login-Ajax-call": 'true'
                    }
                }).success(function (data) {
                    $scope.RoleData = data.data;
                });
                $scope.loading = false;
            }).error(function (data) {
                $scope.error = "An Error has occured while Saving Friend! " + data;
                $scope.loading = false;
            });
        } else {
            alert("Please input required field !!");
        }
    };

});