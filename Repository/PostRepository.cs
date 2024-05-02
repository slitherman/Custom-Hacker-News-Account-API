﻿using Custom_Hacker_News_Account_API.Manual_Mapping;
using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;
using Microsoft.EntityFrameworkCore;

namespace Custom_Hacker_News_Account_API.Repository
{
    public class PostRepository
    {
        public readonly AccountDbContext _dbContext;
        public AccountRepository _accRepo { get; set; }

        public PostRepository(AccountDbContext DbContext, AccountRepository repo )
        {
            _dbContext = DbContext;
            _accRepo = repo;
        }

        public Post CreatePost(CreateAndUpdatePostDTO postToAdd)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                int method = 1;

                PostDTO postDTO = ManualMapper.MapCreatePostDTOToDTO(postToAdd);


                Post post = postDTO.MapDTOToPost();

                _dbContext.Posts.Add(post);
                _accRepo.modifyAccountStats(method, post.AccountId);
                transaction.Commit();
                return post;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Failed to create post and update account statistics.", ex);
            }

        }
        public PostDTO GetPostById(int id)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => p.PostId == id);

            if (post == null)
            {
                throw new ArgumentNullException($"Could not get the specified post {post}");
            }
            var postDTO = post.MapPostToDTO();
            return postDTO;
        }
        public PostDTO DeletePost(int id)
        {
            var post = GetPostById(id);
            if (post == null)
            {
                throw new ArgumentNullException($"Selected post with the id {id} doesnt exist");
            }
            int method = 2;

            _accRepo.modifyAccountStats(method, post.AccountId);

            var PostDTO = post.MapDTOToPost();
            _dbContext.Posts.Remove(PostDTO);
            _dbContext.SaveChanges();
            return post;
        }
      
        public Post UpdatePost(int id, CreateAndUpdatePostDTO postToUpdate)
        {


            var existingPost = GetPostById(id);

            if (existingPost == null)
            {
                throw new ArgumentNullException($"Could not find the specified post with the id {id}");
            }

            try
            {
                existingPost.Title = postToUpdate.Title;
                existingPost.Url = postToUpdate.Url;
          
                PostDTO DTOToPOST = ManualMapper.MapCreatePostDTOToDTO(postToUpdate);

                Post post = DTOToPOST.MapDTOToPost();
                _dbContext.SaveChanges();
                return post;

            }
            catch(Exception ex)
            {
                throw new Exception("Failed up update post", ex);
            }


        }

        public IEnumerable<PostDTO> GetAllPosts()
        {
            return _dbContext.Posts.Select(p => p.MapPostToDTO()).ToList();        
        }

        public IEnumerable<PostDTO> GetAllCommentsInPost()
        {
            return _dbContext.Posts.Include(p => p.Comments).Select(p => p.MapPostToDTO()).ToList();   
        }
        public void UpvoteRecieved(int id)
        {
            var post = GetPostById(id);
            if (post == null)
            {
                throw new ArgumentNullException($"Selected post with the id {id} doesnt exist");
            }
            int method = 3;
            _accRepo.modifyAccountStats(method, post.AccountId);
            _dbContext.SaveChanges();
        }
    }
}
