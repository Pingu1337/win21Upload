using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace win21.Data.UploadServices
{
    public class PostService
    {
        private readonly ILogger<UploadService> _logger;
        private IDbContextFactory<PostContext> _contextFactory;
        private IDbContextFactory<ApplicationDbContext> _UserContextFactory;

        public PostService(ILogger<UploadService> logger, IDbContextFactory<PostContext> contextFactory, IDbContextFactory<ApplicationDbContext> UserContextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
            _UserContextFactory = UserContextFactory;
        }

        public async Task SavePostInfo(Post post)
        {
            try
            {
                await using (var context = await _contextFactory.CreateDbContextAsync())
                {
                    var newPost = new Post
                    {
                        PostName = "",
                        UploadDate = DateTime.Now,
                        PostUrl = "",
                        PostSize = "",
                        UserId = ""

                    };
                    await context.AddAsync(post);
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
            }
        }

        public async Task<List<Post>> GetPostInfo(string id)
        {
            List<Post> postList;
            try
            {
                
                await using (var context = await _contextFactory.CreateDbContextAsync())
                {
                    var _postList = context
                        .Posts
                        .Where(u => u.UserId == id)
                        .ToList();
                    postList = _postList;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return postList;
        }

        public async Task<int?> GetPostId()
        {
            int? id;
            try
            {
                await using (var context = await _contextFactory.CreateDbContextAsync())
                {
                    var post = context
                        .Posts
                        .OrderByDescending(i => i.Id)
                        .FirstOrDefault();

                    if (post != null)
                    {
                        id = post.Id + 1;
                    }
                    else
                    {
                        id = 1;
                    }
                    
                }
            }
            catch (Exception e)
            {
                id = null;
            }

            return id;

        }

        public async Task<IdentityUser> GetUser(string id)
        {
            IdentityUser user;
            await using (var userCtx = await _UserContextFactory.CreateDbContextAsync())
            {
                var identityUser = await userCtx
                    .Users
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();
             
                    user = identityUser;

            }

            return user;
        }
    }
}
