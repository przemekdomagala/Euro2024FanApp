using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Euro2024REST.Data;
using Euro2024REST.Models;

namespace Euro2024REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionsModelController : ControllerBase
    {
        private readonly ContextDb _context;

        public PredictionsModelController(ContextDb context)
        {
            _context = context;
        }

        // GET: api/PredictionsModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Predictions>>> GetPredictions()
        {
            return await _context.Predictions.ToListAsync();
        }

        // GET: api/PredictionsModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Predictions>> GetPredictions(int id)
        {
            var predictions = await _context.Predictions.FindAsync(id);

            if (predictions == null)
            {
                return NotFound();
            }

            return predictions;
        }

        // PUT: api/PredictionsModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPredictions(int id, Predictions predictions)
        {
            if (id != predictions.Id)
            {
                return BadRequest();
            }

            _context.Entry(predictions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PredictionsExists(id))
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

        // POST: api/PredictionsModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Predictions>> PostPredictions(Predictions predictions)
        {
            _context.Predictions.Add(predictions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPredictions", new { id = predictions.Id }, predictions);
        }

        // DELETE: api/PredictionsModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePredictions(int id)
        {
            var predictions = await _context.Predictions.FindAsync(id);
            if (predictions == null)
            {
                return NotFound();
            }

            _context.Predictions.Remove(predictions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PredictionsExists(int id)
        {
            return _context.Predictions.Any(e => e.Id == id);
        }
    }
}
