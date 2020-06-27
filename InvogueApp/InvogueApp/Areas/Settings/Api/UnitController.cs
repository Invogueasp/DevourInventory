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
    public class UnitController : ApiController
    {
        static readonly IUnitFactory unitFactory = new UnitFactorys();
        [HttpPost]
        public List<Unit> GetUnit(int? unitID, string token)
        {
            var unitModal = new List<Unit>();
            if (Constants.Token == token)
            {
                var units = unitFactory.SearchUnit(unitID);
                foreach (var unit in units)
                {
                    var uModal = new Unit();
                    uModal.UnitID = unit.UnitID;
                    uModal.Name = unit.Name;
                    uModal.ShortName = unit.ShortName;
                    uModal.Status = unit.Status;
                    unitModal.Add(uModal);
                }
                return unitModal;
            }
            return unitModal;
        }
    }

    public class Unit
    {
        public int UnitID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Status { get; set; }
    }
}
