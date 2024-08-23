using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAng.API.Models.Domain;
using NetAng.API.Models.DTO;
using NetAng.API.Repositories.Interface;

namespace NetAng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(
            IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository
        )
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        // POST: {apibaseurl}/api/blogposts
        [HttpPost]
        [Authorize(Roles="Writer")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            // DTO to Domain
            var blogPost = new BlogPost
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                IsVisible = request.IsVisible,
                Categories = new List<Category>() // Initialize the collection
            };

            foreach (var categoryId in request.Categories) 
            {
                var existingCategory = await categoryRepository.GetByIdAsync(categoryId);
                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }
                 
            blogPost = await blogPostRepository.CreateAsync(blogPost);

            // Domain to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            }; 

            return Ok(response);
        }

        // GET: {apibaseurl}/api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            // Convert Domain model to DTO
            var response = new List<BlogPostDto>();  
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    IsVisible = blogPost.IsVisible,
                    Categories = blogPost.Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    }).ToList()
                });

            }
            return Ok(response);
        }

        // GET: {apibaseurl}/api/blogposts/{id}
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if (blogPost is null)
            {
                return NotFound();
            }

            // Domain to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        // GET: {apibaseurl}/api/blogposts/{urlHandle}
        [HttpGet("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            // Get BlogPost details from repository
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            if (blogPost is null)
            {
                return NotFound();
            }

            // Domain to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        // PUT: {apibaseurl}/api/blogposts/{id}
        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            // DTO to Domain
            var blogPost = new BlogPost
            {
                Id = id,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                IsVisible = request.IsVisible,
                Categories = new List<Category>() // Initialize the collection
            };

            // Foreach
            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetByIdAsync(categoryGuid);

                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            // Call Repository To Update BlogPost
            var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);

            if (updatedBlogPost is null)
            {
                return NotFound();
            }

            // Domain to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        // DELETE: {apibaseurl}/api/blogposts/{id}
        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPostById([FromRoute] Guid id)
        {

            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);
            if (deletedBlogPost is null)
            {
                return NotFound();
            }
            
            // Domain to DTO
            var response = new BlogPostDto
            {
                Id = deletedBlogPost.Id,
                Title = deletedBlogPost.Title,
                ShortDescription = deletedBlogPost.ShortDescription,
                Content = deletedBlogPost.Content,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                UrlHandle = deletedBlogPost.UrlHandle,
                PublishedDate = deletedBlogPost.PublishedDate,
                Author = deletedBlogPost.Author,
                IsVisible = deletedBlogPost.IsVisible,
            };

            return Ok(response);
        }
    }
}
