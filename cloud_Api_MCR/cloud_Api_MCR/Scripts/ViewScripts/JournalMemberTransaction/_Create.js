var JornalApp = angular.module("JornalApp", ['ui.bootstrap']);

JornalApp.controller("ApiMemberTransactionController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
        
        $(document).on("click", "select.grdAccountNameId", function (e) {
            $(this).select2();
        });
        $(document).on("click", "select.grdSchemeId", function (e) {
            $(this).select2();
        });
        $(document).on("click", "select.grdInternId", function (e) {
            $(this).select2();
        });
    };


    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;

        //Grid view load 
        var cid1 = $('#txtCid').val();
        var uid1 = $('#txtUid').val();
        var tid1 = $('#txtTid').val();
        var transType = "JOUR";
        var monthYear = $('#txtTransMonthYear').val();
        var transNo = $('#txtTransNo').val();
        $http.get('/api/MemberTransaction/Information/', {
            params: {
                userCompanycode: cid1,
                usercode: uid1,
                transType: transType,
                transMonthYear: monthYear,
                transno: transNo,
                transSerial:0
            },
            headers: {
                'Authorization': 'token ' + tid1,
                'Accept': 'application/json',
                "X-Login-Ajax-call": 'true'
            }
        }).success(function (data) {
            $scope.gridData = data.data;
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
            this.newChild.Transdt = $('#txtTransDate').val();
            this.newChild.Transtp = "JOUR";
            this.newChild.Transno = $('#txtTransNo').val();
            this.newChild.Transmy = $('#txtTransMonthYear').val();
            this.newChild.Transfor = $('#txtTransFor').val();
            this.newChild.Transmode = $('#txtTransMode').val();

            this.newChild.Glheadid = $('#ddlAccountNameId').val();
            this.newChild.Schemeid = $('#ddlSchemeId').val();
            this.newChild.Memberid = $('#txtMemberId').val();
            this.newChild.Internid = $('#txtInternId').val();
            this.newChild.Remarks = $('#txtRemarksId').val();
            this.newChild.Amount = $('#txtAmount').val();
            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            if (this.newChild.Transdt !== "" && this.newChild.Transtp !== "" && this.newChild.Transno !== "" && this.newChild.Transmy !== "" && this.newChild.Branchid !== "" && this.newChild.Fwid !== "" && this.newChild.Schemeid !== "" && this.newChild.Memberid !== "" && this.newChild.Internid !== "" && this.newChild.Amount !== "") {
                $http.post('/api/MemberTransaction/Create/MemoSerialWise?userCompanycode=' + this.newChild.Compid +
                                 "&usercode=" + this.newChild.Userid, this.newChild, config).success(function (data, status, headers, config) {

                                     //Grid view load 
                                     var cid2 = $('#txtCid').val();
                                     var uid2 = $('#txtUid').val();
                                     var tid2 = $('#txtTid').val();
                                     var transType2 = "JOUR";
                                     var monthYear2 = $('#txtTransMonthYear').val();
                                     var transNo2 = $('#txtTransNo').val();
                                     $http.get('/api/MemberTransaction/Information/', {
                                         params: {
                                             userCompanycode: cid2,
                                             usercode: uid2,
                                             transType: transType2,
                                             transMonthYear: monthYear2,
                                             transno: transNo2,
                                             transSerial: 0
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

                                     $('#txtRemarksId').val("");
                                     $('#txtAmount').val("");
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
        this.testitem.Transtp = "JOUR";
        this.testitem.Transmy = $('#txtTransMonthYear').val();
        this.testitem.Transno = $('#txtTransNo').val();
       
        var update = $('#txt_Updateid').val();

        if (update === "I" || this.testitem.Transno === "" || this.testitem.Transmy === "") {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var cid3 = $('#txtCid').val();
            var uid3 = $('#txtUid').val();
            var tid3 = $('#txtTid').val();
            var transType3 = "JOUR";
            var monthYear3 = $('#txtTransMonthYear').val();
            var transNo3 = $('#txtTransNo').val();
            $http.get('/api/MemberTransaction/Information/', {
                params: {
                    userCompanycode: cid3,
                    usercode: uid3,
                    transType: transType3,
                    transMonthYear: monthYear3,
                    transno: transNo3,
                    transSerial: 0
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
            $http.post('/api/MemberTransaction/Update?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     alert(data.message);
                                     frien.editMode = false;

                                     //Grid view load 
                                     var cid3 = $('#txtCid').val();
                                     var uid3 = $('#txtUid').val();
                                     var tid3 = $('#txtTid').val();
                                     var transType3 = "JOUR";
                                     var monthYear3 = $('#txtTransMonthYear').val();
                                     var transNo3 = $('#txtTransNo').val();
                                     $http.get('/api/MemberTransaction/Information/', {
                                         params: {
                                             userCompanycode: cid3,
                                             usercode: uid3,
                                             transType: transType3,
                                             transMonthYear: monthYear3,
                                             transno: transNo3,
                                             transSerial: 0
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

        this.testitem.Compid = $('#txtCid').val();
        this.testitem.Userid = $('#txtUid').val();
        this.testitem.Updltude = $('#latlon').val();
        this.testitem.Transtp = "JOUR";
        this.testitem.Transmy = $('#txtTransMonthYear').val();
        this.testitem.Transno = $('#txtTransNo').val();

        var dlete = $('#txt_Deleteid').val();
        if (dlete === "I") {
            alert("Permission Denied!!");
        } else {
            var compid = this.testitem.Compid;
            var transTp = this.testitem.Transtp;
            var transno = this.testitem.Transno;
            var transmy = this.testitem.Transmy;
            var transsl = this.testitem.Transsl;

            var config = {
                headers: {
                    'Authorization': 'token ' + $('#txtTid').val(),
                    'Accept': 'application/json',
                    "X-Login-Ajax-call": 'true'
                }
            };
            $http.post('/api/MemberTransaction/Delete?userCompanycode=' + this.testitem.Compid +
                                 "&usercode=" + this.testitem.Userid, this.testitem, config).success(function (data) {
                                     if (data.success === true) {
                                         $.each($scope.gridData, function (i) {
                                             if ($scope.gridData[i].Compid === compid && $scope.gridData[i].Transtp === transTp && $scope.gridData[i].Transmy === transmy && $scope.gridData[i].Transno === transno && $scope.gridData[i].Transsl === transsl) {
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





    $scope.getMemberName = function (value) {
        var txtIntern = value;
        var txtScheme = this.testitem.Schemeid;
        $.ajax
              ({
                  url: '/Collect/GetMemberName',
                  type: 'GET',
                  dataType: "json",
                  data: { txtInternId: txtIntern, txtSchemeId: txtScheme },
                  success: function (data) {
                      $("input.grdMemberName").val(data.MemberName);
                      $("input.grdMemberId").val(data.MemberId);
                  }
              });

        $("input.grdMemberName").val(" ");
        $("input.grdMemberId").val(" ");
    }



    $scope.GetSummOfAmount = function (gridData) {
        var summ = 0;
        for (var i in gridData) {
            if (gridData.hasOwnProperty(i)) {
                summ = summ + Number(gridData[i].Amount);
            }
        }
        $('#gridTotalAmount').val(summ);
        return summ;
    };
});