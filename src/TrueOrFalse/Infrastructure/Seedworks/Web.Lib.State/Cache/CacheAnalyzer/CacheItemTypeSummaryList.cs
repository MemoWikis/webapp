using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpeakFriend.Utilities.ValueObjects;

namespace Seedworks.Web.State.Analysis
{
    /// <summary>
    /// The Cacheitem
    /// </summary>
    public class CacheItemTypeSummaryList : List<CacheItemTypeSummary>
    {
        /// <summary>
        /// The total size, in case of serialization
        /// </summary>
        public BinarySize TotalSize
        {
            get{ return new BinarySize(this.Sum(summary => summary.Size.Bytes)); }
        }

        public int TotalAmountOfNonSizeableElements
        {
            get { return this.Count(summary => summary.Size.Bytes == 0); }
        }

        public int TotalCount
        {
            get {return this.Sum(summary => summary.Amount);}
        }

        public void Add(DictionaryEntry summary)
        {
            var wrapper = new CacheItemWrapper(summary);
            Add(wrapper.ToCacheItemSummary());
        }

        public new void Add(CacheItemTypeSummary typeSummary)
        {
            var item = GetByType(typeSummary.Type);

            if (item == null)
                base.Add(typeSummary);
            else
            {
                item.Amount += typeSummary.Amount;
                item.Size += typeSummary.Size;
            }
                
        }

        /// <summary>
        /// Returns null if Summary for the given type is not found
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public CacheItemTypeSummary GetByType(Type type)
        {
            return Find(item => item.Type == type);
        }

        public bool ContainsType(Type type)
        {
            return GetByType(type) != null;
        }


    }
}
