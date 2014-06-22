using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse;


public interface ICategoryBase
{
    CategoryType Type { get; }
    Category Category { get; set; }
}