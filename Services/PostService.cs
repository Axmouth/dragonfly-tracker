using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{


    public class PostService : IPostService
    {
        private readonly PgMainDataContext _pgMainDataContext;

        public PostService(PgMainDataContext pgMainDataContext)
        {
            _pgMainDataContext = pgMainDataContext;
        }

        public async Task<List<Post>> GetPostsAsync(GetAllPostsFilter filter = null, PaginationFilter paginationFilter = null)
        {
            var queryable = _pgMainDataContext.Posts.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable.Include(x => x.Tags).ToListAsync().ConfigureAwait(false);
            }

            queryable = AddFiltersOnQuery(filter, queryable);

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Include(x => x.Tags)
                .Skip(skip).Take(paginationFilter.PageSize).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _pgMainDataContext.Posts
                .Include(x => x.Tags)
                .SingleOrDefaultAsync(x => x.Id == postId).ConfigureAwait(false);
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            post.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            await AddNewTags(post).ConfigureAwait(false);
            await _pgMainDataContext.Posts.AddAsync(post);

            var created = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
            await AddNewTags(postToUpdate).ConfigureAwait(false);
            _pgMainDataContext.Posts.Update(postToUpdate);
            var updated = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = new Post { Id = postId };

            _pgMainDataContext.Posts.Attach(post);


            _pgMainDataContext.Posts.Remove(post);
            var deleted = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return deleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _pgMainDataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId).ConfigureAwait(false);

            if (post == null)
            {
                return false;
            }

            if (post.UserId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _pgMainDataContext.Tags.AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            tag.Name = tag.Name.ToLower();
            var existingTag = await _pgMainDataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tag.Name).ConfigureAwait(false);
            if (existingTag != null)
                return true;

            await _pgMainDataContext.Tags.AddAsync(tag);
            var created = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            return created > 0;
        }

        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await _pgMainDataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower()).ConfigureAwait(false);
        }

        public async Task<bool> DeleteTagAsync(string tagName)
        {
            var tag = await _pgMainDataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower()).ConfigureAwait(false);

            if (tag == null)
                return true;

            var postTags = await _pgMainDataContext.PostTags.Where(x => x.TagName == tagName.ToLower()).ToListAsync().ConfigureAwait(false);

            _pgMainDataContext.PostTags.RemoveRange(postTags);
            _pgMainDataContext.Tags.Remove(tag);
            return await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false) > postTags.Count;
        }

        private async Task AddNewTags(Post post)
        {
            foreach (var tag in post.Tags)
            {
                var existingTag =
                    await _pgMainDataContext.Tags.SingleOrDefaultAsync(x =>
                        x.Name == tag.TagName).ConfigureAwait(false);
                if (existingTag != null)
                    continue;

                await _pgMainDataContext.Tags.AddAsync(new Tag
                { Name = tag.TagName, CreatedOn = DateTime.UtcNow, CreatorId = post.UserId });
            }
        }



        private static IQueryable<Post> AddFiltersOnQuery(GetAllPostsFilter filter, IQueryable<Post> queryable)
        {
            if (!string.IsNullOrEmpty(filter?.UserId))
            {
                queryable = queryable.Where(x => x.UserId == filter.UserId);
            }

            return queryable;
        }
    }
}
