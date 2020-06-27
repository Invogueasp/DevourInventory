using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Models;
using BLL.Common;
using BLL.Factory;
using BLL.Factory.Security;
using BLL.Interfaces;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.db;
using DAL.Helper;

namespace ICBS.Controllers
{
    public class UserController : Controller
    {
        // GET: User

        private IGenericFactory<SEC_UserInformation> _userFactory;
        private IGenericFactory<SEC_SecurityQuestion> _questionFactory;
        private IGenericFactory<SEC_Password> _passwordFactory;
        private IGenericFactory<SEC_UserGroup> _userGroupFactory;
        private Result result;
        string tableName = "User";
        public ActionResult User()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "User");
                if (tblUserActionMapping.Select == true)
                {
                    ViewBag.CallingForm = "Security";
                    ViewBag.CallingForm1 = "User";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = "false";
            return Redirect("/#!/");
        }
        public ActionResult EditUser()
        {
            DefaultLoad();
            return View();
        }
        

        
        public ActionResult LoadAllUser()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId != 0)
                {
                    _userFactory = new UserFactory();
                    var users = _userFactory.GetAll().Select(x => new
                    {
                        x.ID,
                        x.UserGroupID,
                        x.UserFullName,
                        x.Address,
                        x.PhoneNo,
                        User = x.UserName,
                        x.IsActive,
                        x.Email,
                        x.UserName,
                        //x.PersonnelId,
                        x.CompanyID,
                        x.BranchID,
                        x.DepartmentID
                        //x.Hr_PersonnelBasics.PersonnelName
                    }).ToList();
                    return Json(users.OrderBy(x => x.UserFullName));
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ActiveDeActiveUser(int id, bool status)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                if (userGroupId != 0)
                {
                    ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                    PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "User");
                    if (tblUserActionMapping.Edit)
                    {
                        _userGroupFactory = new UserGroupFactory();
                        _userFactory = new UserFactory();
                        int userId = Convert.ToInt32(dictionary[3].Id);
                        SEC_UserInformation user = _userFactory.FindBy(x => x.ID == userId).FirstOrDefault();
                        SEC_UserGroup userGroup = _userGroupFactory.FindBy(x => x.ID == user.UserGroupID).FirstOrDefault();
                        if (userGroup != null && userGroup.IsAdmin)
                        { 
                            _userFactory = new UserFactory();
                            SEC_UserInformation tblUserInformation = _userFactory.FindBy(x => x.ID == id).FirstOrDefault();
                            if (tblUserInformation != null)
                            {
                                tblUserInformation.IsActive = status;
                                _userFactory.Edit(tblUserInformation);
                            }
                            _userFactory.Save();
                            if (status)
                            {
                                return Json(new { success = true, message = "Sucessifuly activeted the User" }, JsonRequestBehavior.AllowGet);
                            }
                            return Json(new { success = true, message = "Sucessifuly de-activeted the User" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { success = false, message = "You are not Admin User" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "You has no permission for edit" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllUserGroups()
        {
            try
            {
                _userGroupFactory = new UserGroupFactory();
                var userGroups = _userGroupFactory.GetAll().Select(x => new
                {
                    x.ID,
                    x.Name
                }).ToList();
                return Json(new { success = true, data = userGroups.OrderBy(x => x.Name) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //public JsonResult loadEmployee()
        //{
        //    try
        //    {
        //        //_employeeFactory = new PersonnelInforFactory();
        //        var employee = _employeeFactory.GetAll().Select(x => new
        //        {
        //            x.PersonnelId,
        //            PersonnelName = x.PersonnelName + " - " + x.PersonnelCode
        //        }).ToList();
        //        return Json(employee, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception exception)
        //    {
        //        return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult CreateUserSave(UserModel user)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userId = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
                if (userId != 0)
                {
                    _userFactory = new UserFactory();
                    SEC_UserInformation isDuplicate = _userFactory.FindBy(x => x.UserName.ToLower().Trim() == user.UserName.ToLower().Trim()).FirstOrDefault();
                    if (isDuplicate == null)
                    {
                        return CreateUser(user, userId);
                       
                    }
                    else
                    {
                        result.message = "user already exists";
                        result.isSucess = false;
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                Session["logInSession"] = "false";
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private JsonResult CreateUser(UserModel user, int userId)
        {            
            result = new Result();
            _questionFactory = new QuestionFactory();
            _passwordFactory = new UserPasswordFactory();

            var question = new SEC_SecurityQuestion();
            question.ID = Guid.NewGuid();

            var password = new SEC_Password();
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int companyId = Convert.ToInt32(dictionary[9].Id == "" ? 0 : Convert.ToInt32(dictionary[9].Id));

            //question.SecurityQuestion = user.SecurityQuestion;
            //question.SecutiryAnswer = user.SecurityQueAns;
            question.SecurityQuestion = "Which one is your favorite sport Team";
            question.SecutiryAnswer = "BD";
            question.CreatedBy = userId;
            question.CreatedDate = DateTime.Now;
            _questionFactory.Add(question);
            result = _questionFactory.Save();

           
            if (result.isSucess)
            {
                
                var encription = new Encription();
                password.ID = Guid.NewGuid();
                password.NewPassword = encription.Encrypt(user.Password);
                password.OldPassword = "";
                password.IsSelfChanged = false;
                password.CreatedBy = userId;
                password.CreatedDate = DateTime.Now;
                _passwordFactory.Add(password);
                result = _passwordFactory.Save();
            }
            var userInformation = new SEC_UserInformation();
            if (result.isSucess)
            {
                userInformation.CompanyID = companyId;
                userInformation.BranchID = user.BranchID;
                userInformation.UserFullName = user.UserFullName;
                userInformation.UserName = user.UserName.ToLower().Trim();
                userInformation.Address = user.Address;
                userInformation.Email = user.EMail;
                userInformation.PhoneNo = user.PhoneNo;
                userInformation.SecurityQuestionID = question.ID;
                userInformation.PasswordID = password.ID;
                userInformation.IsEMailVerified = false;
                userInformation.IsPhoneNoVerified = false;
                userInformation.IsActive = true;
                userInformation.CreatedBy = userId;
                userInformation.DepartmentID = user.DepartmentID;
                userInformation.CreatedDate = DateTime.Now;
                userInformation.UserGroupID = user.UserGroupID;
                //userInformation.CustomerID = user.CustomerID;
                _userFactory.Add(userInformation);
                result = _userFactory.Save();
            }

            if (result.isSucess)
            {
                result.message = result.SaveSuccessfull(tableName);
                Session["logInSession"] = "true";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult UpdateUserForm(UserModel user)
        {
            try
            {
                result = new Result();
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userId = Convert.ToInt32(dictionary[3].Id);
                if (userId != 0)
                {

                    _userFactory = new UserFactory();
                    //TBLA_USER_INFORMATION aUserInformation = _userFactory.FindBy(x => x.UserName == user.UserName.ToLower().Trim() && x.CompanyID == companyId).Where(x => x.ID != user.ID).FirstOrDefault();
                    var aUserInformation = _userFactory.FindBy(x => x.UserName == user.UserName.ToLower().Trim()).Where(x => x.ID != user.ID).FirstOrDefault();
                    if (aUserInformation == null)
                    {
                        SEC_UserInformation userInformation = _userFactory.FindBy(x => x.ID == user.ID).FirstOrDefault();
                        if (userInformation != null)
                        {
                            userInformation.UpdatedDate = DateTime.Now;
                            userInformation.UpdatedBy = userId;
                            userInformation.Email = user.EMail;
                            userInformation.PhoneNo = user.PhoneNo;
                            userInformation.UserGroupID = user.UserGroupID;
                            userInformation.UserFullName = user.UserFullName;
                            userInformation.UserName = user.UserName;
                            //userInformation.PersonnelId = user.PersonnelId;
                            _userFactory.Edit(userInformation);
                        }
                        result = _userFactory.Save();
                        //if (result.isSucess)
                        //{
                        //    _questionFactory = new QuestionFactory();
                        //    TBLA_SECURITY_QUESTION securityQuestion = _questionFactory.FindBy(x => x.ID == user.SecurityQuestionID).FirstOrDefault();
                        //    if (securityQuestion != null && (securityQuestion.SecurityQuestion == user.SecurityQuestion || securityQuestion.SecutiryAnswer == user.SecurityQueAns))
                        //    {
                        //        securityQuestion.SecurityQuestion = user.SecurityQuestion;
                        //        securityQuestion.SecutiryAnswer = user.SecurityQueAns;
                        //        securityQuestion.ModifiedDate = DateTime.Now;
                        //        securityQuestion.ModifiedBy = userId;
                        //        _questionFactory.Edit(securityQuestion);
                        //        result = _questionFactory.Save();
                        //    }
                        //}

                        if (result.isSucess)
                        {
                            result.message = result.UpdateSuccessfull(tableName);
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }


                    }
                    return Json(new { success = false, message = "Your entared user name are duplicate please chose another name" }, JsonRequestBehavior.AllowGet);
                }
                Session["logInSession"] = "false";
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LoadUserGroups()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int companyId = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
                if (companyId != 0)
                {
                    _userGroupFactory = new UserGroupFactory();
                    var userGroups = _userGroupFactory.GetAll().Select(x => new
                    {
                        id = x.Name,
                        Group = x.Name
                    }).ToList();
                    return Json(new { success = true, data = userGroups.OrderBy(x => x.Group) }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { success = false, message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UserRoleChange(int id, string userRole)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int companyId = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
                if (companyId != 0)
                {
                    _userGroupFactory = new UserGroupFactory();
                    _userFactory = new UserFactory();
                    int userId = Convert.ToInt32(dictionary[3].Id);
                    SEC_UserInformation user = _userFactory.FindBy(x => x.ID == userId).FirstOrDefault();
                    SEC_UserGroup userGroup = _userGroupFactory.FindBy(x => x.ID == user.UserGroupID).FirstOrDefault();
                    if (userGroup != null && userGroup.IsAdmin)
                    {
                        SEC_UserGroup role = _userGroupFactory.FindBy(x => x.Name == userRole).FirstOrDefault();
                        _userFactory = new UserFactory();
                        SEC_UserInformation tblUserInformation = _userFactory.FindBy(x => x.ID == id).FirstOrDefault();
                        if (tblUserInformation != null)
                        {
                            tblUserInformation.UserGroupID = role.ID;
                            _userFactory.Edit(tblUserInformation);
                        }
                        _userFactory.Save();
                        return Json(new { success = true, message = "Sucessifuly changed the user role" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "You are not Admin User" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(int id)
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int companyId = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
                if (companyId != 0)
                {
                    int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
                    ISecurityFactory _securityLogInFactory = new SecurityFactorys();
                    PagePermissionVM tblUserActionMapping = _securityLogInFactory.GetCrudPermission(userGroupId, "User");
                    if (tblUserActionMapping.Delete)
                    {
                        _userFactory = new UserFactory();
                        _userFactory.Delete(x => x.ID == id);
                        _userFactory.Save();
                        return Json(new { success = true, message = "Deleted Successfuly" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "You has no delete permission" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "LogOut" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Another page use this User data" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UserCreate()
        {
            
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "User");
                if (tblUserActionMapping.Create)
                {
                    DefaultLoad();
                    return View();
                }
            }
            Session["logInSession"] = "false";
            return Redirect("/#!/");
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Security";
            ViewBag.CallingForm1 = "User";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/User";
        }
    }
}