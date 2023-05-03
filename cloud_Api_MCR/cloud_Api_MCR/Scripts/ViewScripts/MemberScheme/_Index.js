var MemberSchemeApp = angular.module("MemberSchemeApp", ['ui.bootstrap']);

MemberSchemeApp.controller("ApiMemberSchemeController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {
        $scope.loading = true;
        event.preventDefault();

        // Grid view load 
        var cid = $('#txtCid').val();
        var uid = $('#txtUid').val();
        var tid = $('#txtTid').val();
        var memberid = $('#ddlMemberId option:selected').val();
        $http.get('/api/MemberScheme/List/', {
            params: {
                userCompanycode: cid,
                usercode: uid,
                memberId: memberid
            },
            headers: {
                'Authorization': 'token ' + tid,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            if (data.data.length === 0) {
                $scope.gridData = null;
            } else {
                $scope.gridData = data.data;
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
        var memberid1 = $('#ddlMemberId option:selected').val();
        $http.get('/api/MemberScheme/List/', {
            params: {
                userCompanycode: cid1,
                usercode: uid1,
                memberId: memberid1
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
            this.newChild.Memberid = $('#ddlMemberId option:selected').val();

            this.newChild.Schemeid = $('#ddlSchemeId').val();
            this.newChild.Internid = $('#txtInternId').val();
            this.newChild.Schemeefdt = $('#txtFromDateId').val();
            this.newChild.Schemeetdt = $('#txtToDateId').val();
            this.newChild.Remarks = $('#txtRemarksId').val();
            this.newChild.Status = $('#ddlStatusId').val();
            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            if (this.newChild.Schemeid !== "" && this.newChild.Internid !== "" && this.newChild.Status !== "" && this.newChild.Schemeefdt !== "" && this.newChild.Schemeetdt !== "") {
                $http.post('/api/MemberScheme/Create?userCompanycode=' + this.newChild.Compid +
                                 "&usercode=" + this.newChild.Userid, this.newChild, config).success(function (data, status, headers, config) {

                                     //Grid view load 
                                     var cid2 = $('#txtCid').val();
                                     var uid2 = $('#txtUid').val();
                                     var tid2 = $('#txtTid').val();
                                     var memberid2 = $('#ddlMemberId option:selected').val();
                                     $http.get('/api/MemberScheme/List/', {
                                         params: {
                                             userCompanycode: cid2,
                                             usercode: uid2,
                                             memberId: memberid2
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid2,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.gridData = data.data;
                                         $scope.loading = false;
                                     });

                                     $('#txtInternId').val("");
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
        this.testitem.Memberid = $('#ddlMemberId option:selected').val();

        this.testitem.Schemeefdt = $('#gridFromDateId').val();
        this.testitem.Schemeetdt =$('#gridToDateId').val();

        var update = $('#txt_Updateid').val();

        if (update === "I" || this.testitem.Internid === "" || this.testitem.Schemeefdt === "" || this.testitem.Schemeetdt === "") {
            alert("Permission Denied!!");
            frien.editMode = false;

            $('#gridFromDateId').val("");
            $('#gridFromDateId').val("");

            //Grid view load 
            var cid3 = $('#txtCid').val();
            var uid3 = $('#txtUid').val();
            var tid3 = $('#txtTid').val();
            var memberid3 = $('#ddlMemberId option:selected').val();
            $http.get('/api/MemberScheme/List/', {
                params: {
                    userCompanycode: cid3,
                    usercode: uid3,
                    memberId: memberid3
                },
                headers: {
                    'Authorization': 'token ' + tid3,
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            }).success(function (data) {
                $scope.gridData = data.data;
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
            $http.post('/api/MemberScheme/Update?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     alert(data.message);
                                     frien.editMode = false;

                                     $('#gridFromDateId').val("");
                                     $('#gridFromDateId').val("");

                                     //Grid view load 
                                     var cid3 = $('#txtCid').val();
                                     var uid3 = $('#txtUid').val();
                                     var tid3 = $('#txtTid').val();
                                     var memberid3 = $('#ddlMemberId option:selected').val();
                                     $http.get('/api/MemberScheme/List/', {
                                         params: {
                                             userCompanycode: cid3,
                                             usercode: uid3,
                                             memberId: memberid3
                                         },
                                         headers: {
                                             'Authorization': 'token ' + tid3,
                                             'Accept': 'application/json',
                                             "X-Login-Ajax-call": 'true'
                                         }
                                     }).success(function (data) {
                                         $scope.gridData = data.data;
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

        $('#gridFromDateId').val("");
        $('#gridFromDateId').val("");

        this.testitem.Compid = $('#txtCid').val();
        this.testitem.Userid = $('#txtUid').val();
        this.testitem.Updltude = $('#latlon').val();
        this.testitem.Memberid = $('#ddlMemberId option:selected').val();

        var dlete = $('#txt_Deleteid').val();
        if (dlete === "I") {
            alert("Permission Denied!!");
        } else {
            var compid = this.testitem.Compid;
            var memberid = this.testitem.Memberid;
            var schemesl = this.testitem.Schemesl;

            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            $http.post('/api/MemberScheme/Delete?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     if (data.success === true) {
                                         $.each($scope.gridData, function (i) {
                                             if ($scope.gridData[i].Compid === compid && $scope.gridData[i].Memberid === memberid && $scope.gridData[i].Schemesl === schemesl) {
                                                 $scope.gridData.splice(i, 1);
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