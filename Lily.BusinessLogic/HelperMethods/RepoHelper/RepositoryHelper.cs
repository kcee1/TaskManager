using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lily.BusinessLogic.HelperMethods.RepoHelper
{
    public static class RepositoryHelper
    {

        public static void validateId(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("id");
            
        }

    }
}
