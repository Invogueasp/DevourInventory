using BLL.Interfaces;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Factory
{
    public class CountryFactory : GenericFactory<DevourInvEntities, SET_Country>
    {
        
    }

    public class CountryFactorys : ICommonFactory
    {
        private IGenericFactory<SET_Country> countryFactory;

        public List<SET_Country> SearchCountry(int? countryID)
        {
            var list = new List<SET_Country>();
            countryFactory = new CountryFactory();
            try
            {                
                if (countryID > 0)
                {
                    list = countryFactory.FindBy(x => x.CountryID == countryID).ToList();
                }
                else
                {
                    list = countryFactory.GetAll().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

    }
}
