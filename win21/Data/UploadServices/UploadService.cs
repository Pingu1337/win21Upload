using ByteSizeLib;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using win21.Pages;
using Microsoft.Extensions.Logging;
using win21.Data;


namespace win21.Data.UploadServices
{

    public class UploadService
    {
        private readonly ILogger<UploadService> _logger;
        private IDbContextFactory<ApplicationDbContext> _contextFactory;

        public UploadService(ILogger<UploadService> logger, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
        }


        public async Task<string> GetUserIDasync(string userName)
        {

            string userID;
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var user = ctx.Users
                    .Where(u => u.UserName == userName)
                    .FirstOrDefault();
                userID = user.Id;
            }

            return userID;
        }


    }
}
