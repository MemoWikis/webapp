using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;

public class EditQuestionModel_Categories_Exist : IRegisterAsInstancePerLifetime
{
    public string MissingCategory { get; private set; }

    public bool Yes(EditQuestionModel model)
    {
        MissingCategory = model.Categories
            .FirstOrDefault(categoryName =>
                            ServiceLocator.Resolve<CategoryRepository>()
                                .GetByName(categoryName) == null);
        return MissingCategory == null;
    }

}