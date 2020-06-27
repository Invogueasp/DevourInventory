using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public int ID { get; set; }
        //public int PersonnelId { get; set; }
        public int CompanyID { get; set; }
        public int DepartmentID { get; set; }
        public string CompanyName { get; set; }
        public int BranchID { get; set; }
        public string UserFullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public string PhoneNo { get; set; }
        public int HowDoyouKnow { get; set; }
        public Guid PasswordID { get; set; }
        public Guid SecurityQuestionID { get; set; }
        public bool IsEMailVerified { get; set; }
        public bool IsPhoneNoVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<Guid> UpdateddBy { get; set; }
        public int UserGroupID { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityQueAns { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
        public string BOCode { get; set; }
        public string TradingCode { get; set; }
        public bool IsClient { get; set; }
        public string Message { get; set; }
        public int CustomerID { get; set; }
    }
}