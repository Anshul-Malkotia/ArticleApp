using ArticleApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArticleApp.Controllers
{
    [ApiController]
    [Route("api/v1/articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.ContentTable);

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ContentTable article)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var user = await _context.Users.FindAsync(userId);
            article.UserId = userId;
            article.CreatedBy = user.Username;

            _context.ContentTable.Add(article);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = article.Id }, article);
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] ContentTable article)
        {
            var existingArticle = await _context.ContentTable.FindAsync(id);
            if (existingArticle == null) return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            if (existingArticle.UserId != userId) return Forbid();

            existingArticle.Title = article.Title;
            existingArticle.Content = article.Content;
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = article.Id }, article);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.ContentTable.FindAsync(id);
            if (article == null) return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            if (article.UserId != userId) return Forbid();

            _context.ContentTable.Remove(article);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
