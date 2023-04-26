﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Tabloid.Models;
using Tabloid.Repositories;

namespace Tabloid.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        public PostController(IPostRepository postRepository, IUserProfileRepository userProfileRepository)
        {
            _postRepository = postRepository;
            _userProfileRepository = userProfileRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {

            return Ok(_postRepository.GetAll());
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            var post = _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
    
        [Authorize]
        [HttpGet("userposts")]
        public IActionResult GetUserPosts()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return Ok(_postRepository.GetByUserId(firebaseUserId));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post(Post post)
        {
            UserProfile user = GetCurrentUserProfile();

            post.CreateDateTime = DateTime.Now;
            post.PublishDateTime = DateTime.Now;
            post.UserProfileId = user.Id;
            _postRepository.Add(post);
            return CreatedAtAction(
                nameof(GetPostById), new { post.Id }, post);
        }

        private UserProfile GetCurrentUserProfile()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userProfileRepository.GetByFirebaseUserId(firebaseUserId);
        }
    }
}