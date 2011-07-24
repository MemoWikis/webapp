using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class CategoryRepository : RepositoryDb<Category>
    {
        public CategoryRepository(ISession session) : base(session)
        {
        }

        public override void Create(Category category)
        {
            foreach (var classification in category.Classifications)
                if (classification.Id == 0)
                {
                    classification.DateCreated = DateTime.Now;
                    classification.DateModified = DateTime.Now;

                    foreach(var classificationItem in classification.Items)
                    {
                        if(classification.Id == 0)
                        {
                            classificationItem.DateCreated = DateTime.Now;
                            classificationItem.DateModified = DateTime.Now;   
                        }
                    }
                }

            base.Create(category);
            Flush();
        }
    }
}
