//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrganizationCore
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string RegisteredPerson { get; set; }
        public byte OrganizationMode { get; set; }
        public string OrganizationAddress { get; set; }
        public string PosTCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public byte[] OrganizationLogo { get; set; }
        public string Web { get; set; }
    }
}
