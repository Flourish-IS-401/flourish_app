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
    [Route("api/custom-affirmations")]
    public class CustomAffirmationsController : ControllerBase
    {
        private readonly FlourishDbContext _context;

        public CustomAffirmationsController(FlourishDbContext context)
        {
            _context = context;
        }

        // GET /api/custom-affirmations
        [HttpGet]
        public async Task<ActionResult<List<CustomAffirmation>>> GetAll()
        {
            var items = await _context.CustomAffirmations
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/custom-affirmations/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomAffirmation>> GetById(Guid id)
        {
            var item = await _context.CustomAffirmations.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}

