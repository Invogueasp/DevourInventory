using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class ParametersModel
    {
        public int? ProjectID { get; set; }
        public int? PersonnelId { get; set; }
        public string ProjectName { get; set; }
        public string BoqNumber { get; set; }
        public int? WorkID { get; set; }
        public int? BoqMasterID { get; set; }
        public bool? WithValue { get; set; }
    }
}