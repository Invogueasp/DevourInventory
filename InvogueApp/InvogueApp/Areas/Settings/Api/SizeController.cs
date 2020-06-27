using Application.Common;
using BLL.Factory.Settings;
using BLL.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InvogueApp.Areas.Settings.Api
{
    public class SizeController : ApiController
    {
        static readonly ISizeFactory sizeFactory = new SizeFactorys();
        [HttpPost]
        public List<Size> GetSize(int? sizeID, string token)
        {
            var sizeModal = new List<Size>();
            if (Constants.Token == token)
            {
                var sizes = sizeFactory.SearchSize(sizeID);
                foreach (var size in sizes)
                {
                    var sModal = new Size();
                    sModal.SizeID = size.SizeID;
                    sModal.Name = size.Name;
                    sModal.Code = size.Code;                   
                    sModal.Description = size.Description;
                    sModal.Status = size.Status;
                    sizeModal.Add(sModal);
                }
                return sizeModal;
            }
            return sizeModal;
        }
    }

    public class Size
    {
        public int SizeID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    
    }
}
