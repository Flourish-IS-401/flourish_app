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
    [Route("api/baby-activities")]
    public class BabyActivitiesController : ControllerBase
    {
        private readonly FlourishDbContext _context;

        public BabyActivitiesController(FlourishDbContext context)
        {
            _context = context;
        }

        // GET /api/baby-activities?type=nap
        [HttpGet]
        public async Task<ActionResult<List<BabyActivity>>> GetAll([FromQuery] string? type = null)
        {
            IQueryable<BabyActivity> q = _context.BabyActivities;

            if (!string.IsNullOrWhiteSpace(type))
            {
                q = q.Where(x => x.Type == type);
            }

            var items = await q
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/baby-activities/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BabyActivity>> GetById(Guid id)
        {
            var item = await _context.BabyActivities.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}

