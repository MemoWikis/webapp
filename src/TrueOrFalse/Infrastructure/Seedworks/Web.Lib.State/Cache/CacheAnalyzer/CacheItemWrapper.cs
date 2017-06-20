using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SpeakFriend.Utilities.ValueObjects;

namespace Seedworks.Web.State.Analysis
{
    public struct CacheItemWrapper
    {
        private DictionaryEntry _dictionaryEntry;

        public CacheItemWrapper(DictionaryEntry dictionaryEntry)
        {
            _dictionaryEntry = dictionaryEntry;
        }

        /// <summary>
        /// The approximate size assumed the value are serialized. 
        /// It ignores completely references.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// For unmanaged types the use of sizeof() and Marshal.SizeOf() could be considered.
        /// </remarks>
        public BinarySize GetSize()
        {
            if(!_dictionaryEntry.Value.GetType().IsSerializable)
                return new BinarySize(0);

            try
            {
                var formatter = new BinaryFormatter();
                var memoryStream = new MemoryStream();

                formatter.Serialize(memoryStream, _dictionaryEntry.Value);
                return new BinarySize(memoryStream.Length);
            }
            //its not worth the effort to use reflection for checking the object graph 
            //if every member and its childs are serializable
            catch (SerializationException) 
            {
                return new BinarySize(0);
            }

        }

        public CacheItemTypeSummary ToCacheItemSummary()
        {
            return new CacheItemTypeSummary
                       {
                           Amount = 1,
                           Size = GetSize(),
                           Type = _dictionaryEntry.Value.GetType()
                       };
        }
    }
}
