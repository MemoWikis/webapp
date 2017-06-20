using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.Persistence
{
    public interface IPager
    {
        /// <summary>
        /// The total amount of items the query would return without paging
        /// </summary>
        int TotalItems { get; set; }

        bool QueryAll { get; set; }

        /// <summary>
        /// The amount of pages of the current result
        /// </summary>
        int PageCount { get; }

        int PageSize { get; set; }
        int CurrentPage { get; set; }
        int FirstResult { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }

        /// <summary>
        /// Lower Bound of the Current Page
        /// </summary>
        int LowerBound { get; }

        /// <summary>
        /// Upper Bound of the Current Page
        /// </summary>
        int UpperBound { get; }

        int NextLowerBound { get; }
        int NextUpperBound { get; }

        /// <summary>
        /// SetTrueOrInactive the page as an positive offset. 
        /// </summary>
        /// <param name="amountOfPages">The amount of pages to advance.</param>
        void NextPage(int amountOfPages);

        void NextPage();

        /// <summary>
        /// SetTrueOrInactive the page as an negative offset.
        /// </summary>
        /// <param name="amountOfPages">The amount of page pages to go back.</param>
        void PreviousPage(int amountOfPages);

        void PreviousPage();
        void LastPage();
        void FirstPage();

    	bool HasNextPage();
    	bool HasPreviousPage();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalToShow"></param>
        /// <returns></returns>
        List<int> GetPages(int totalToShow);
    }
}
