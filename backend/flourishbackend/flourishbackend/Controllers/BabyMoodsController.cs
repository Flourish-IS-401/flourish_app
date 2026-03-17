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
    [Route("api/baby-moods")]
    public class BabyMoodsController : ControllerBase
    {
        private readonly FlourishDbContext _context;

        public BabyMoodsController(FlourishDbContext context)
        {
            _context = context;
        }

        // GET /api/baby-moods
        [HttpGet]
        public async Task<ActionResult<List<BabyMood>>> GetAll()
        {
            var items = await _context.BabyMoods
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/baby-moods/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BabyMood>> GetById(Guid id)
        {
            var item = await _context.BabyMoods.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}

