﻿using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync(GetAllPostsFilter filter = null, PaginationFilter paginationFilter = null);

        Task<bool> CreatePostAsync(Post post);

        Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);

        Task<bool> UserOwnsPostAsync(Guid postId, Guid userId);

        Task<List<Tag>> GetAllTagsAsync();

        Task<bool> CreateTagAsync(Tag tag);

        Task<Tag> GetTagByNameAsync(string tagName);

        Task<bool> DeleteTagAsync(string tagName);
    }
}
