using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Settings;
using DAL.db;
using DAL.Helper;

namespace BLL.Factory.Settings
{
    public class CompanyBranchFactory : GenericFactory<DevourInvEntities, SET_CompanyBranch>
    {

    }
   
    public class CompanyBranchFactorys: ICompanyBranchFactory
    {
        private DevourInvEntities db;
        private IGenericFactory<SET_CompanyBranch> _companyBranchFactory;
        private Result _result;
        public CompanyBranchFactorys()
        {
            db = new DevourInvEntities();
        }        

        public List<SET_CompanyBranch> SearchCompanyBranch(int? id)
        {
            try
            {
                _companyBranchFactory = new CompanyBranchFactory();
                var list = new List<SET_CompanyBranch>();
                if (id > 0)
                {
                    list = _companyBranchFactory.FindBy(x => x.BranchID == id).ToList();
                }
                else
                {
                    list = _companyBranchFactory.GetAll().ToList();
                }

                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
    }
}
