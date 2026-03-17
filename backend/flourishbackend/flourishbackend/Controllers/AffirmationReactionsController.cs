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
    [Route("api/affirmation-reactions")]
    public class AffirmationReactionsController : ControllerBase
    {
        private readonly FlourishDbContext _context;

        public AffirmationReactionsController(FlourishDbContext context)
        {
            _context = context;
        }

        // GET /api/affirmation-reactions?affirmationId=xyz
        [HttpGet]
        public async Task<ActionResult<List<AffirmationReaction>>> GetAll([FromQuery] string? affirmationId = null)
        {
            IQueryable<AffirmationReaction> q = _context.AffirmationReactions;

            if (!string.IsNullOrWhiteSpace(affirmationId))
            {
                q = q.Where(x => x.AffirmationId == affirmationId);
            }

            var items = await q
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/affirmation-reactions/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AffirmationReaction>> GetById(Guid id)
        {
            var item = await _context.AffirmationReactions.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}

