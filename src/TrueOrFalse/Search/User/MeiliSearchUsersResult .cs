using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Search
{
    public class MeiliSearchUsersResult : ISearchUsersResult
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MeiliSearchUsersResult(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public int Count { get; set; }

        public List<int> UserIds { get; set; } = new();

        public IList<UserCacheItem> GetUsers() => EntityCache.GetUsersByIds(UserIds, _httpContextAccessor, _webHostEnvironment); 
    }
}
