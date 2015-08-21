namespace DAL.Domain.Criteria
{
    using System.Collections.Generic;

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
