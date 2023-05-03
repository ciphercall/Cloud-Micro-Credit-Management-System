using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cloud_Api_MCR.Controllers.ASL_Promotion;
using cloud_Api_MCR.DataAccess.API;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.ASL;
using cloud_Api_MCR.Models.ASL_DTO;

namespace cloud_Api_MCR.Controllers.Api_ASL_Promotion
{
    public class ApiUploadGroupController : ApiController
    {
        private DatabaseDbContext db;

        public ApiUploadGroupController()
        {
            db = new DatabaseDbContext();
        }



        [Route("api/ApiUploadGroup/GetGroupData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<UploadGroupDTO> GetGroupData(string Compid)
        {
            Int64 compid = Convert.ToInt64(Compid);
            var find_GridData = (from t1 in db.AslPromotionalGroupsDbSet
                                 where t1.Compid == compid
                                 select new
                                 {
                                     t1.Id,
                                     t1.Compid,
                                     t1.GroupId,
                                     t1.GroupNm,

                                     t1.Insipno,
                                     t1.Insltude,
                                     t1.Instime,
                                     t1.Insuserid,
                                 }).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new UploadGroupDTO
                {
                    Count = 1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    yield return new UploadGroupDTO
                    {
                        Id = s.Id,
                        Compid = Convert.ToInt64(s.Compid),
                        GroupId = Convert.ToInt64(s.GroupId),
                        GroupNm = s.GroupNm,
                        Insuserid = s.Insuserid,
                        Insltude = s.Insltude,
                        Instime = s.Instime,
                        Insipno = s.Insipno,
                    };
                }
            }
        }




        //[ActionName("Add")]
        [HttpPost]
        [Route("api/ApiUploadGroup/Add")]
        public HttpResponseMessage AddData(UploadGroupDTO model)
        {
            AslPromotionalGroups uploadGroup = new AslPromotionalGroups();

            var check_data = (from n in db.AslPromotionalGroupsDbSet where n.Compid == model.Compid && n.GroupNm == model.GroupNm select n).ToList();
            if (check_data.Count == 0)
            {
                var find_data = (from n in db.AslPromotionalGroupsDbSet where n.Compid == model.Compid select n.GroupId).ToList();
                if (find_data.Count == 0)
                {
                    uploadGroup.GroupId = Convert.ToInt64(Convert.ToString(model.Compid) + "01");
                }
                else
                {
                    Int64 find_Max_MCATID = Convert.ToInt64((from n in db.AslPromotionalGroupsDbSet where n.Compid == model.Compid select n.GroupId).Max());
                    uploadGroup.GroupId = find_Max_MCATID + 1;
                }

                uploadGroup.Compid = model.Compid;
                uploadGroup.GroupNm = model.GroupNm;
                uploadGroup.Userpc = UserlogAddress.UserPc(); 
                uploadGroup.Insipno = UserlogAddress.IpAddress();
                uploadGroup.Instime = Convert.ToDateTime(UserlogAddress.Timezone(DateTime.Now));
                uploadGroup.Insuserid = model.Insuserid;
                uploadGroup.Insltude = Convert.ToString(model.Insltude);

                db.AslPromotionalGroupsDbSet.Add(uploadGroup);
                db.SaveChanges();

                model.Id = uploadGroup.Id;
                model.Compid = uploadGroup.Compid;
                model.GroupId = Convert.ToInt64(uploadGroup.GroupId);
                model.GroupNm = uploadGroup.GroupNm;
                model.Userpc = uploadGroup.Userpc;
                model.Insipno = uploadGroup.Insipno;
                model.Instime = uploadGroup.Instime;
                model.Insuserid = uploadGroup.Insuserid;
                model.Insltude = Convert.ToString(uploadGroup.Insltude);

                //Log data save from UploadGroup Tabel
                UploadGroupController groupController = new UploadGroupController();
                groupController.Insert_UploadGroup_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.GroupId = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }






        //[ActionName("Update")]
        [HttpPost]
        [Route("api/ApiUploadGroup/Update")]
        public HttpResponseMessage UpdateData(UploadGroupDTO model)
        {
            var check_data = (from n in db.AslPromotionalGroupsDbSet where n.Compid == model.Compid && n.GroupNm == model.GroupNm select n).ToList();
            if (check_data.Count == 0)
            {
                var data_find = (from n in db.AslPromotionalGroupsDbSet where n.Id == model.Id && n.Compid == model.Compid && n.GroupId == model.GroupId select n).ToList();

                foreach (var item in data_find)
                {
                    item.Id = model.Id;
                    item.Compid = model.Compid;
                    item.GroupId = Convert.ToInt64(model.GroupId);
                    item.GroupNm = model.GroupNm;

                    item.Userpc = UserlogAddress.UserPc();
                    item.Updipno = UserlogAddress.IpAddress();
                    item.Updltude = Convert.ToString(model.Insltude);
                    item.Updtime = Convert.ToDateTime(UserlogAddress.Timezone(DateTime.Now));
                    item.Upduserid = Convert.ToInt16(model.Insuserid);
                }
                db.SaveChanges();

                //Log data save from MediMst Tabel
                UploadGroupController groupController = new UploadGroupController();
                groupController.update_UploadGroup_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.GroupId = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }





        //[ActionName("Delete")]
        [HttpPost]
        [Route("api/ApiUploadGroup/Delete")]
        public HttpResponseMessage DeleteData(UploadGroupDTO model)
        {
            AslPromotionalGroups deleteModel = new AslPromotionalGroups();
            deleteModel.Id = model.Id;
            deleteModel.Compid = model.Compid;
            deleteModel.GroupId = Convert.ToInt64(model.GroupId);

            var findChildData = (from n in db.AslPromotionalContactDbSet
                                 where n.GroupId == deleteModel.GroupId && n.Compid == deleteModel.Compid
                                 select n).ToList();

            UploadGroupDTO GroupObj = new UploadGroupDTO();

            if (findChildData.Count != 0)
            {
                GroupObj.GetChildDataForDeleteMasterCategory = 1; // True (child data found)
            }
            else
            {
                deleteModel = db.AslPromotionalGroupsDbSet.Find(deleteModel.Id, deleteModel.Compid, deleteModel.GroupId);
                db.AslPromotionalGroupsDbSet.Remove(deleteModel);
                db.SaveChanges();

                //Log data save from MediMst Tabel
                UploadGroupController groupController = new UploadGroupController();
                groupController.Delete_UploadGroup_LogData(model);
                groupController.Delete_UploadGroup_LogDelete(model);
            }
            return Request.CreateResponse(HttpStatusCode.OK, GroupObj);
        }
    }
}
