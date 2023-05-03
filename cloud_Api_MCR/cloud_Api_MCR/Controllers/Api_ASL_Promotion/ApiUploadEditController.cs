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
    public class ApiUploadEditController : ApiController
    {
        private DatabaseDbContext db;

        public ApiUploadEditController()
        {
            db = new DatabaseDbContext();
        }



        
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/ApiUploadEdit/GetContactData")]
        public IEnumerable<UploadContactDTO> GetContactData(string compId, string groupId)
        {
            Int64 compid = Convert.ToInt64(compId);
            Int64 GroupId = Convert.ToInt64(groupId);
            var find_GridData = (from contact in db.AslPromotionalContactDbSet
                                 join groupNm in db.AslPromotionalGroupsDbSet on contact.Compid equals groupNm.Compid
                                 where contact.Compid == compid && contact.GroupId == GroupId &&
                                 contact.GroupId == groupNm.GroupId //&& mediCare.GHEADID==ghead.GHEADID
                                 select new
                                 {
                                     contact.Id,
                                     contact.Compid,
                                     contact.GroupId,
                                     groupNm.GroupNm,
                                     contact.Name,
                                     contact.Email,
                                     contact.MobNo1,
                                     contact.MobNo2,
                                     contact.Address,

                                     contact.Insipno,
                                     contact.Insltude,
                                     contact.Instime,
                                     contact.Insuserid,
                                 }).OrderBy(e => e.Id).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new UploadContactDTO
                {
                    Count = 1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    yield return new UploadContactDTO
                    {
                        Id = s.Id,
                        CompId = Convert.ToInt64(s.Compid),
                        FromGroupId = Convert.ToInt64(s.GroupId),
                        ToGroupId = Convert.ToInt64(s.GroupId),
                        ToGroupNm = Convert.ToString(s.GroupNm),
                        Name = s.Name,
                        Email = s.Email,
                        MobNo1 = s.MobNo1,
                        MobNo2 = s.MobNo2,
                        Address = s.Address,
                        Insuserid = s.Insuserid,
                        Insltude = s.Insltude,
                        Instime = s.Instime,
                        Insipno = s.Insipno,
                    };
                }
            }
        }



        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }



        private bool BdNumberValidate(string number)
        {
            try
            {
                if (number.Length > 13 || number.Length < 13)
                {
                    return false;
                }
                else
                {
                    string operatorCode = number.Substring(0, 5);
                    switch (operatorCode)
                    {
                        case "88018":
                        case "88017":
                        case "88019":
                        case "88016":
                        case "88011":
                        case "88015":
                            return true; //all of the operator in case is return true
                            break;
                        default:
                            return false; //other operator code return false
                    }
                }
            }
            catch
            {
                return false;
            }
        }




        //[ActionName("Update")]
        [HttpPost]
        [Route("api/ApiUploadEdit/Update")]
        public HttpResponseMessage UpdateData(UploadContactDTO model)
        {
            if (IsValidEmail(model.Email) || BdNumberValidate(model.MobNo1) || BdNumberValidate(model.MobNo2))
            {
                var check_EMAIL_data = (from n in db.AslPromotionalContactDbSet where n.Id != model.Id && n.Compid == model.CompId && n.GroupId == model.ToGroupId && n.Email == model.Email select n).ToList();
                var check_MObileNo1 = (from n in db.AslPromotionalContactDbSet where n.Id != model.Id && n.Compid == model.CompId && n.GroupId == model.ToGroupId && (n.MobNo1 == model.MobNo1 || n.MobNo2 == model.MobNo1) select n).ToList();
                //var check_MObileNo1_datainMOBNO2 = (from n in db.AslPromotionalContactDbSet where n.ID != model.ID && n.COMPID == model.COMPID && n.GroupId == model.ToGroupId && n.MobNo2 == model.MobNo1 select n).ToList();
                var check_MObileNo2 = (from n in db.AslPromotionalContactDbSet where n.Id != model.Id && n.Compid == model.CompId && n.GroupId == model.ToGroupId && (n.MobNo1 == model.MobNo2 || n.MobNo2 == model.MobNo2) select n).ToList();
                //var check_MObileNo2_datainMOBNO1 = (from n in db.AslPromotionalContactDbSet where n.ID != model.ID && n.COMPID == model.COMPID && n.GroupId == model.ToGroupId && n.MOBNO1 == model.MobNo2 select n).ToList();
                if (model.FromGroupId == model.ToGroupId && check_EMAIL_data.Count == 0 && check_MObileNo1.Count == 0 && check_MObileNo2.Count == 0)
                {
                    //Update Logic
                    var data_find = (from n in db.AslPromotionalContactDbSet
                                     where n.Id == model.Id && n.Compid == model.CompId && n.GroupId == model.ToGroupId
                                     select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.Id = model.Id;
                        item.Compid = model.CompId;
                        item.GroupId = Convert.ToInt64(model.ToGroupId);
                        item.Name = model.Name;
                        if (IsValidEmail(model.Email))
                        {
                            item.Email = model.Email;
                        }
                        else
                        {
                            item.Email = null;
                        }
                        if (model.MobNo1 != null && BdNumberValidate(model.MobNo1))
                        {
                            item.MobNo1 = model.MobNo1;
                        }
                        else
                        {
                            item.MobNo1 = null;
                        }
                        if (model.MobNo2 != null && BdNumberValidate(model.MobNo2))
                        {
                            item.MobNo2 = model.MobNo2;
                        }
                        else
                        {
                            item.MobNo2 = null;
                        }
                        item.Address = model.Address;

                        item.Userpc = UserlogAddress.UserPc();
                        item.Updipno = UserlogAddress.IpAddress();
                        item.Updltude = Convert.ToString(model.Insltude);
                        item.Updtime = Convert.ToDateTime(UserlogAddress.Timezone(DateTime.Now));
                        item.Upduserid = Convert.ToInt16(model.Insuserid);

                    }
                    db.SaveChanges();

                    //Log data saved from UploadContact Tabel (update)
                    UploadEditController controller = new UploadEditController();
                    controller.Update_LogData(model);

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                    return response;
                }
                else if (model.FromGroupId != model.ToGroupId && check_EMAIL_data.Count == 0 && check_MObileNo1.Count == 0 && check_MObileNo2.Count == 0)
                {
                    AslPromotionalContact contactsExchanged = new AslPromotionalContact();

                    contactsExchanged.Compid = model.CompId;
                    contactsExchanged.GroupId = Convert.ToInt64(model.ToGroupId);
                    contactsExchanged.Name = model.Name;
                    if (IsValidEmail(model.Email))
                    {
                        contactsExchanged.Email = model.Email;
                    }
                    else
                    {
                        contactsExchanged.Email = null;
                    }
                    if (model.MobNo1 != null && BdNumberValidate(model.MobNo1))
                    {
                        contactsExchanged.MobNo1 = model.MobNo1;
                    }
                    else
                    {
                        contactsExchanged.MobNo1 = null;
                    }
                    if (model.MobNo2 != null && BdNumberValidate(model.MobNo2))
                    {
                        contactsExchanged.MobNo2 = model.MobNo2;
                    }
                    else
                    {
                        contactsExchanged.MobNo2 = null;
                    }
                    contactsExchanged.Address = model.Address;

                    contactsExchanged.Userpc = UserlogAddress.UserPc();
                    contactsExchanged.Insipno = UserlogAddress.IpAddress();
                    contactsExchanged.Instime = Convert.ToDateTime(UserlogAddress.Timezone(DateTime.Now));
                    contactsExchanged.Insuserid = model.Insuserid;
                    contactsExchanged.Insltude = Convert.ToString(model.Insltude);

                    db.AslPromotionalContactDbSet.Add(contactsExchanged);
                    db.SaveChanges();

                    //Log data saved from UploadContact Tabel (exchange data)
                    UploadEditController controller = new UploadEditController();
                    controller.Insert_Exchange_LogData(model);

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                    return response;
                }
                else
                {
                    model.CheckPreviousData = 1;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                    return response;
                }
            }
            else
            {
                model.CheckValidation = 1;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }








        //[ActionName("Delete")]
        [HttpPost]
        [Route("api/ApiUploadEdit/Delete")]
        public HttpResponseMessage DeleteData(UploadContactDTO model)
        {
            AslPromotionalContact deleteModel = new AslPromotionalContact();
            deleteModel.Id = model.Id;
            deleteModel.Compid = model.CompId;

            deleteModel = db.AslPromotionalContactDbSet.Find(deleteModel.Id, deleteModel.Compid);
            db.AslPromotionalContactDbSet.Remove(deleteModel);
            db.SaveChanges();

            //Log data save from GheadMst Tabel
            UploadEditController controller = new UploadEditController();
            controller.Delete_Exchange_LogData(model);
            controller.Delete_Exchange_LogDelete(model);


            UploadContactDTO Obj = new UploadContactDTO();
            return Request.CreateResponse(HttpStatusCode.OK, Obj);
        }
    }
}
