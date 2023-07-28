using Microsoft.AspNetCore.Authorization;

namespace ShopOnline.Areas.Identity.AuthorizationHandler
{
    public class GenZrequirement : IAuthorizationRequirement
    {
        public int MinYear { get; }

        public int MaxYear { get; }

        public GenZrequirement(int _minYear = 1997, int _maxYear = 2012)
        {
            MinYear = _minYear;
            MaxYear = _maxYear;
        }
    }
}
