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
        public CategoryRepository(ISession session)
            : base(session)
        {
        }

        public override void Create(Category category)
        {
            InitializeNewClassifications(category);
            base.Create(category);
            Flush();
        }

        public override void Update(Category category)
        {
            InitializeNewClassifications(category);
            base.Update(category);
            Flush();
        }

        private static void InitializeNewClassifications(Category category)
        {
            foreach (var classification in category.Classifications.Where(classification => classification.Id == 0))
            {
                classification.DateCreated = DateTime.Now;
                classification.DateModified = DateTime.Now;

                foreach (var classificationItem in classification.Items.Where(classificationItem => classificationItem.Id == 0))
                {
                    classificationItem.DateCreated = DateTime.Now;
                    classificationItem.DateModified = DateTime.Now;
                }
            }
        }
    }
}
