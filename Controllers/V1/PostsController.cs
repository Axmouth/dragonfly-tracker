﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DragonflyTracker.Cache;
using DragonflyTracker.Contracts.V1;
using DragonflyTracker.Contracts.V1.Requests;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Domain;
using DragonflyTracker.Extensions;
using DragonflyTracker.Helpers;
using DragonflyTracker.Services;

namespace DragonflyTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PostsController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        [Cached(600)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPostsQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllPostsFilter>(query);
            var posts = await _postService.GetPostsAsync(filter, pagination).ConfigureAwait(false);
            var postsResponse = _mapper.Map<List<PostResponse>>(posts);
            
            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<PostResponse>(postsResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, postsResponse);
            return Ok(paginationResponse);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId()).ConfigureAwait(false);

            if (!userOwnsPost)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this post" }));
            }

            var post = await _postService.GetPostByIdAsync(postId).ConfigureAwait(false);
            post.Name = request.Name;

            var updated = await _postService.UpdatePostAsync(post).ConfigureAwait(false);

            if(updated)
                return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId()).ConfigureAwait(false);

            if (!userOwnsPost)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this post" }));
            }
            
            var deleted = await _postService.DeletePostAsync(postId).ConfigureAwait(false);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        [Cached(600)]
        public async Task<IActionResult> Get([FromRoute]Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId).ConfigureAwait(false);

            if (post == null)
                return NotFound();

            return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var newPostId = Guid.NewGuid();
            var post = new Post
            {
                Id = newPostId,
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId(),
                Tags = postRequest.Tags.Select(x=> new PostTag{PostId = newPostId, TagName = x}).ToList()
            };
            
            await _postService.CreatePostAsync(post).ConfigureAwait(false);
            
            var locationUri = _uriService.GetUri(post.Id.ToString());
            return Created(locationUri, new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }
    }
}