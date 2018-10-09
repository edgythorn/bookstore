using BooksStore.Interfaces;
using BooksStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BooksStore.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private const int FILE_REQUEST_SIZE_LIMIT = 1048576; // 1MB

        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));
        }

        [HttpGet]
        [HttpGet("orderby/{field}/{sort}")]
        [HttpGet("page/{pageIndex}/{pageSize}")]
        [HttpGet("orderby/{field}/{sort}/page/{pageIndex}/{pageSize}")]
        public async Task<ActionResult<BooksPage>> Get([FromRoute] Pagination pagination, [FromRoute] SortOrder sortOrder)
        {
            var result = await _booksService.GetBooksAsync(pagination, sortOrder);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetAsync(Guid id)
        {
            var result = await _booksService.GetBookAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [RequestSizeLimit(FILE_REQUEST_SIZE_LIMIT)]
        public async Task<IActionResult> Post([FromForm] Book book, IFormFile file)
        {
            if (file != null)
            {
                using (var stream = file.OpenReadStream())
                {
                    await _booksService.UpdateBookAsync(book, stream, file.ContentType);
                }
            }
            else
            {
                await _booksService.UpdateBookAsync(book);
            }

            return Ok();
        }

        [HttpPut]
        [RequestSizeLimit(FILE_REQUEST_SIZE_LIMIT)]
        public async Task<IActionResult> PutAsync([FromForm] Book book, IFormFile file)
        {
            Guid id;
            if (file != null)
            {
                using (var stream = file.OpenReadStream())
                {
                    id = await _booksService.CreateBookAsync(book, stream, file.ContentType);
                }
            }
            else
            {
                id = await _booksService.CreateBookAsync(book);
            }

            return Created($"{Request.Path}/{id.ToString()}", book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _booksService.DeleteBookAsync(id);
            return Ok();
        }
    }
}
