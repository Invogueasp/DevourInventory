using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICommonFactory
    {
        List<SET_Country> SearchCountry(int? countryID);
    }
}
