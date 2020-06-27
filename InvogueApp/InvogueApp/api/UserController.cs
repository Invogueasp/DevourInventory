using Application.Common;
using BLL.Factory.Security;
using BLL.Interfaces;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InvogueApp.api
{
    public class UserController : ApiController
    {
        private IGenericFactory<SEC_UserInformation> _userFactory;
        [HttpPost]
        public UserModal GetUser(string userName, string token)
        {
            var userModal = new UserModal();
            if (Constants.Token == token)
            {
                _userFactory = new UserFactory();

                var user = _userFactory.FindBy(x => x.UserName == userName).FirstOrDefault();
                if (user != null)
                {
                    userModal.UserFullName = user.UserFullName;
                    userModal.Email = user.Email;
                    userModal.BirthDate = user.CreatedDate;
                    userModal.PhoneNo = user.PhoneNo;
                }
            }
            return userModal;
        }
    }

    public class UserModal
    {
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNo { get; set; }
    }
}