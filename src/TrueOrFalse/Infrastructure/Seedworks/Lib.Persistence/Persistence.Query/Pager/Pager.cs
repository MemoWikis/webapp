using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.Persistence
{
    [Serializable]
    public class Pager : IPager
    {
    	public Pager()
    	{
    		TotalItems = 0;
    		QueryAll = true;
    	}

    	/// <summary>
        /// The total amount of items the query would return without paging
        /// </summary>
        public int TotalItems { get; set; }

        public bool QueryAll { get; set; }

        //////////////////////////////
        // Result Related
        //////////////////////////////

        /// <summary>
        /// The amount of pages of the current result
        /// </summary>
        public int PageCount
        {
            get
            {
                if (QueryAll)
                    return 1;

                return (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
        }

        //////////////////////////////
        // Query Related
        //////////////////////////////

        protected int _pageSize = 10;
        public int PageSize
        {
            get
            {
                if (_isInSingleItemMode)
                    return 3;

                return _pageSize;
            }
            set
            {
                _pageSize = value;
                QueryAll = false;
            }
        }

        protected int _currentPage = 1;
        public int CurrentPage
        {
            get
            {
                if (_isInSingleItemMode)
                    throw new InvalidOperationException("CurrentPage is invalid when in NavigationPagerMode.");

                if (_currentPage < 1 || PageCount == 0)
                    return 1;

                if (_currentPage > PageCount)
                    return PageCount;

                return _currentPage;
            }
            set { _currentPage = value; }
        }

        public int FirstResult
        {
            get
            {
                if (_isInSingleItemMode)
                {
                    if (IsFirstPage)
                        return 0;

                    return _navigationPagerResultIndex - 1;
                }

                return PageSize * (CurrentPage - 1);
            }
        }

        public bool QueryExpiredRequests = false;

        public bool IsFirstPage
        {
            get
            {
                if (_isInSingleItemMode)
                    return _navigationPagerResultIndex == 0;

                return CurrentPage == 1;
            }
        }

        public bool IsLastPage
        {
            get
            {
                if (_isInSingleItemMode)
                    return _navigationPagerResultIndex == TotalItems - 1;

                // If no results, then the first page is also the last, even though PageCount == 0.
                if (PageCount == 0)
                    return true;

                return CurrentPage == PageCount;
            }
        }

        /// <summary>
        /// Lower Bound of the Current Page
        /// </summary>
        public int LowerBound
        {
            get { return GetLowerBoundOfPage(CurrentPage); }
        }

        /// <summary>
        /// Upper Bound of the Current Page
        /// </summary>
        public int UpperBound
        {
            get { return GetUpperBoundOfPage(CurrentPage); }
        }

        public int NextLowerBound
        {
            get
            {
                if (!IsLastPage)
                    return GetLowerBoundOfPage(CurrentPage + 1);

                return GetLowerBoundOfPage(CurrentPage);
            }
        }

        public int NextUpperBound
        {
            get
            {
                if (!IsLastPage)
                    return GetUpperBoundOfPage(CurrentPage + 1);

                return GetUpperBoundOfPage(CurrentPage);
            }
        }

        private int GetLowerBoundOfPage(int page)
        {
            return ((page - 1) * PageSize) + 1;
        }

        private int GetUpperBoundOfPage(int page)
        {
            int result = page * PageSize;

            if (result > TotalItems)
                result = TotalItems;

            return result;
        }

        /// <summary>
        /// Flip forward by a positive number of pages. 
        /// </summary>
        /// <param name="amountOfPages">The amount of pages to advance.</param>
        public void NextPage(int amountOfPages)
        {
            for (int i = 0; i < amountOfPages; i++)
                NextPage();
        }


        public void NextPage()
        {
            if (CurrentPage + 1 <= PageCount)
                CurrentPage++;
            else
                CurrentPage = PageCount;
        }

        /// <summary>
        /// Flip backward by a positive number of pages.
        /// </summary>
        /// <param name="amountOfPages">The amount of page pages to go back.</param>
        public void PreviousPage(int amountOfPages)
        {
            for (int i = 0; i < amountOfPages; i++)
                PreviousPage();
        }

        public void PreviousPage()
        {
            if (CurrentPage - 1 > 0)
                CurrentPage--;
            else
                CurrentPage = 1;
        }

        public void LastPage()
        {
            CurrentPage = PageCount;
        }

        public void FirstPage()
        {
            CurrentPage = 1;
        }


        /// <summary>
        /// 
        /// </summary>
        protected bool _isInSingleItemMode = false;

        /// <summary>
        /// Is die 
        /// </summary>
        protected int _navigationPagerResultIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalToShow"></param>
        /// <returns></returns>
        public List<int> GetPages(int totalToShow)
        {
            var halfTotalToShow = (int)Math.Floor(totalToShow / 2m);

            if (PageCount <= totalToShow)
                return GetPages(1, PageCount);

            if ((CurrentPage - halfTotalToShow) <= 0)
                return GetPages(1, totalToShow);

            if (CurrentPage + halfTotalToShow > PageCount)
                return GetPages(PageCount - totalToShow + 1, totalToShow);

            return GetPages(CurrentPage - halfTotalToShow, totalToShow);
        }


        private List<int> GetPages(int startPage, int totalToShow)
        {
            var result = new List<int>();

            for (int currentPage = startPage; currentPage < totalToShow + startPage; currentPage++)
                result.Add(currentPage);

            return result;
		}

		public Pager SetItemsPerPage(int itemsPerPage)
		{
			PageSize = itemsPerPage;
			return this;
		}

		public bool HasNextPage()
		{
			return !IsLastPage;
		}          
        
		public bool HasPreviousPage()
		{
			return !IsFirstPage;
		}
    }
}
