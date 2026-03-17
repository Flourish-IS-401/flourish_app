using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flourishbackend.Data;
using Flourish.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flourishbackend.Controllers
{
    [ApiController]
    [Route("api/mood-entries")]
    public class MoodEntriesController : ControllerBase
    {
        private readonly FlourishDbContext _context;

        public MoodEntriesController(FlourishDbContext context)
        {
            _context = context;
        }

        // GET /api/mood-entries?date=YYYY-MM-DD
        [HttpGet]
        public async Task<ActionResult<List<MoodEntry>>> GetAll([FromQuery] string? date = null)
        {
            IQueryable<MoodEntry> q = _context.MoodEntries;

            if (!string.IsNullOrWhiteSpace(date))
            {
                q = q.Where(x => x.Date == date);
            }

            var items = await q
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/mood-entries/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MoodEntry>> GetById(Guid id)
        {
            var item = await _context.MoodEntries.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}

