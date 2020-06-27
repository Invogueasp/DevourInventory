using DAL.db;
//using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class BranchModel
    {
        //-------------- Start:: Class Model List Declare --------------
        public List<OrganizationCore> ListOrganizationCore { get; set; }
        //------------- End:: Class Model List Declare ------------ 

        //----------- Start:: Class Model Object Declare ----------
        public virtual OrganizationCore objOrganizationCore { get; set; }
        //----------- End:: Class Model Object Declare ----------
    }
}