using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using cloud_Api_MCR.Models;
using cloud_Api_MCR.Models.Api;

namespace cloud_Api_MCR.DataAccess.API
{
    public class TokenInfo
    {

        public static UserTokenModel.UserInformation CollectUserInfo(string loginid, string password)
        {
            DatabaseDbContext db = new DatabaseDbContext();
            UserTokenModel.UserInformation userInformation = new UserTokenModel.UserInformation();
            var getData =(from m in db.AslUsercoDbSet where m.Loginid == loginid && m.Loginpw == password select m).ToList();
            foreach (var get in getData)
            {
                userInformation.CompanyId = get.Compid.ToString();
                userInformation.UserId = get.Userid.ToString();
                userInformation.UserName = get.Usernm;
                userInformation.LoginId = get.Loginid;
            }
            return userInformation;
        }


        public static string GenerateToken(UserTokenModel.TokenUserInfo userInfo)
        {
            string datetime = UserlogAddress.Timezone(DateTime.Now).ToString(CultureInfo.InvariantCulture);

            //creating a unique token containing a time stamp
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());

            var chars = userInfo.UserName + token + userInfo.UserCreateDate+ datetime + token;
            chars = SpecialCharRemove(chars);
            var stringChars = new char[100];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            token = new string(stringChars);
            token = token + userInfo.UserId;
            //return new String(stringChars);
            return token;
        }



        public static string SpecialCharRemove(string character)
        {
            return character.Replace(" ", "").Replace("/", "").Replace(":", "").Replace(".", "").Replace("+", "").Replace("-", "").Replace("%", "");
        }


        public static string TokenExpireDateIncrement(DateTime dt, int days)
        {
            string incremendate = dt.AddDays(days).ToString(CultureInfo.InvariantCulture);
            return incremendate;
        }



        public static bool SaveToken(string companyid, string userid, string token, string expeiredate, string password)
        {
            try
            {
                DatabaseDbContext db = new DatabaseDbContext();
                Int64 uid = Convert.ToInt64(userid);
                Int64 cid = Convert.ToInt64(companyid);
                var findUserToken = (from m in db.AslTokenDbSet where m.Token== token select m).ToList();
                if (findUserToken.Count != 0)
                {
                    UserTokenModel.DataWithToken dataWithToken = new UserTokenModel.DataWithToken();
                    dataWithToken.Data = TokenInfo.CollectUserInfo(userid, password);

                    UserTokenModel.TokenUserInfo tokenUserInfo = new UserTokenModel.TokenUserInfo();
                    tokenUserInfo.UserId = dataWithToken.Data.UserId;
                    tokenUserInfo.UserName = dataWithToken.Data.UserName;
                    tokenUserInfo.UserCreateDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);

                    string tokens = TokenInfo.GenerateToken(tokenUserInfo);
                    
                    var getUserToken = (from m in db.AslTokenDbSet where m.Compid == cid && m.Userid == uid select m).ToList();
                    foreach (var model in getUserToken)
                    {
                        model.Token = tokens;
                        model.ExpireDate = Convert.ToDateTime(expeiredate);
                    }
                    db.SaveChanges();
                }
                else
                {
                    var getUserToken = (from m in db.AslTokenDbSet where m.Compid == cid && m.Userid == uid select m).ToList();
                    foreach (var model in getUserToken)
                    {
                        model.Token = token;
                        model.ExpireDate = Convert.ToDateTime(expeiredate);
                    }
                    db.SaveChanges();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
         
        }

        



        public static bool TokenCheck(Int64 companyCode, Int64 usercode, string token)
        {
            DatabaseDbContext db = new DatabaseDbContext();
            var findUserData =
                (from m in db.AslTokenDbSet
                    where m.Compid == companyCode && m.Userid == usercode
                    select new {m.Token, m.ExpireDate}).ToList();
            string apitoken = ""; string tokenExpireDate = "";
            try
            {
                foreach (var getData in findUserData)
                {
                    apitoken = getData.Token.ToString();
                    tokenExpireDate = getData.ExpireDate.ToString();
                }
              

                DateTime transdate = DateTime.Parse(tokenExpireDate);
                DateTime dateTimeNow = UserlogAddress.Timezone(DateTime.Now);

                if (transdate > dateTimeNow && apitoken == token)
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }






   
}
