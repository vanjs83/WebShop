using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Api
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewContentController : ControllerBase
    {
        private readonly DbShop _context;

        public ReviewContentController(DbShop context)
        {
            _context = context;
        }

        // GET: api/ReviewContent
        [HttpGet]  
        public IEnumerable<ReviewContent> GetReviewContent()
        {
            return _context.ReviewContent.ToList();
        }

       
        // GET: api/ReviewContent/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<ReviewContent>> GetReviewContent([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            var reviewContent = await _context.ReviewContent.Where(x => x.ProductId == id).ToListAsync();

            if (reviewContent == null)
            {
                return null;
            }

            return reviewContent;
        }

        // PUT: api/ReviewContent/5
        [HttpPut("Put/{id}")]
        public async Task<IActionResult> PutReviewContent([FromRoute] Guid id, [FromBody] ReviewContent reviewContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reviewContent.ReviewContentId)
            {
                return BadRequest();
            }

            _context.Entry(reviewContent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewContentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ReviewContent
        [HttpPost]
     
        public async Task<IActionResult> PostReviewContent([FromForm] ReviewContent reviewContent)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            reviewContent.CreatedDate = DateTime.Today;
            reviewContent.ReviewContentId = Guid.NewGuid();
           await   _context.ReviewContent.AddAsync(reviewContent);
          //  await  _context.SaveChangesAsync();

            return CreatedAtAction("GetReviewContent", new { id = reviewContent.ReviewContentId }, reviewContent);
        }

        // DELETE: api/ReviewContent/5
        [HttpDelete("{id}")]
      //  [HttpGet]
      //  [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteReviewContent([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewContent = await _context.ReviewContent.FindAsync(id);
            if (reviewContent == null)
            {
                return NotFound();
            }

            _context.ReviewContent.Remove(reviewContent);
         //   await _context.SaveChangesAsync();

            return Ok(reviewContent);
        }

        private bool ReviewContentExists(Guid id)
        {
            return _context.ReviewContent.Any(e => e.ReviewContentId == id);
        }
    }
}