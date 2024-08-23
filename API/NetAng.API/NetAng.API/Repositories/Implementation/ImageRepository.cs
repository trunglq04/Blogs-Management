using Microsoft.EntityFrameworkCore;
using NetAng.API.Data;
using NetAng.API.Models.Domain;
using NetAng.API.Repositories.Interface;

namespace NetAng.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await _context.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            // 1-Store the Image to API/Images 
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath,"Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // 2-Update the database
            // https://netang.com/images/somefilename.jpg

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
            
            blogImage.Url = urlPath;
            
            await _context.BlogImages.AddAsync(blogImage);
            await _context.SaveChangesAsync();

            return blogImage;
        }
    }
}
