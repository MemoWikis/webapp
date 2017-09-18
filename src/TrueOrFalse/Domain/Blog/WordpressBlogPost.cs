using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Quartz.Util;

public class WordpressBlogPost
{
    private class WordpressBlogPostPropertyRendered
    {
        public string Rendered;
    }

    public class WordpressBlogPostEmbedded
    {
        /* Properties are included in JSON when API is used with "?_embed". 
         * This way, the info about media (e.g. featured image for post, author) is directly included in the output. 
         * Otherwise, you would need to fetch it via the media id with a separate query.
         */

        public class EmbeddedAuthor
        {
            public int Id;
            public string Name;
        }

        public class MediaSize
        {
            [JsonProperty("source_url")]
            public string SourceUrl;
            public int Width;
            public int Height;
        }

        public class MediaDetailsType
        {
            public int Width;
            public int Height;

            [JsonProperty("sizes")]
            public Dictionary<string, MediaSize> Sizes;
        }

        public class EmbeddedWpFeaturedMedia
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("caption")]
            private WordpressBlogPostPropertyRendered _caption { get; set; }

            public String Caption => _caption.Rendered;

            [JsonProperty("alt_text")]
            public String AltText;

            [JsonProperty("source_url")]
            public String UrlFullSize;

            [JsonProperty("media_details")]
            public MediaDetailsType MediaDetails { get; set; }

        }

        [JsonProperty("wp:featuredmedia")]
        public IList<EmbeddedWpFeaturedMedia> FeaturedMedia;

        [JsonProperty("author")]
        public IList<EmbeddedAuthor> Authors;

    }



    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("date")]
    public DateTime DateCreated { get; set; }

    //To use a more fancy version to directly access sub-property "rendered" see https://stackoverflow.com/questions/33088462/can-i-specify-a-path-in-an-attribute-to-map-a-property-in-my-class-to-a-child-pr
    [JsonProperty("title")]
    private WordpressBlogPostPropertyRendered _title { get; set; }

    public string Title => _title.Rendered;

    [JsonProperty("link")]
    public string Url { get; set; }

    //[JsonProperty("content")]
    //public string Content { get; set; }

    [JsonProperty("excerpt")]
    private WordpressBlogPostPropertyRendered _excerpt { get; set; }

    public string Excerpt => _excerpt.Rendered;

    [JsonProperty("_embedded")]
    public WordpressBlogPostEmbedded Embedded { get; set; }

    public string AuthorName
    {
        get
        {
            var author = Embedded.Authors.FirstOrDefault();
            if (author != null) return author.Name;
            return "";
        }
    }

    public string FeaturedImageMediumSizedUrl
    {
        get
        {
            var embeddedWpFeaturedMedia = Embedded
                .FeaturedMedia.FirstOrDefault();
            if (embeddedWpFeaturedMedia != null)
                return embeddedWpFeaturedMedia
                    .MediaDetails
                    .Sizes.TryGetAndReturn("medium")
                    .SourceUrl;
            return "";
        }
    }



    //[JsonProperty("categories")]
    //public Dictionary<string, object> Categories { get; set; }

    //public List<string> CategoryNames
    //{
    //    get
    //    {
    //        return Categories.Select(x => x.Key).ToList();
    //    }
    //}
}

