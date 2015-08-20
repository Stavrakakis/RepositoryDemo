using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Criteria
{
    public class UserCriteria
    {
        private List<int> ids = new List<int>();

        public IList<int> Ids 
        {
            get
            {
                return this.ids;
            }
        }

        public UserCriteria ById(int id)
        {
            this.ids.Add(id);

            return this;
        }

    }
}
