using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class ImageMetaData : DomainEntity
    {
        public virtual ImageType Type { get; set; }
        
        /// <summary>E.g questionId, questionSetId, ... </summary>
        public virtual int TypeId { get; set; }
        public virtual ImageSource Source { get; set; }
        public virtual string SourceUrl { get; set; }
        public virtual string ApiResult { get; set; }
        public virtual string ApiHost { get; set; }
        public virtual int UserId { get; set; }
        public virtual string AuthorParsed { get; set; }
        public virtual string DescriptionParsed { get; set; }
        public virtual string Markup { get; set; }
        public virtual License MainLicense { get; set; }
        public virtual string AllRegisteredLicenses { get; set; }
    }
}