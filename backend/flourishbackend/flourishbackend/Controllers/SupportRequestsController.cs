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
    [Route("api/support-requests")]
    public class SupportRequestsController : ControllerBase
    {
        private readonly FlourishDbContext _context;

        public SupportRequestsController(FlourishDbContext context)
        {
            _context = context;
        }

        // GET /api/support-requests?isCustom=true
        [HttpGet]
        public async Task<ActionResult<List<SupportRequest>>> GetAll([FromQuery] bool? isCustom = null)
        {
            IQueryable<SupportRequest> q = _context.SupportRequests;

            if (isCustom.HasValue)
            {
                q = q.Where(x => x.IsCustom == isCustom.Value);
            }

            var items = await q
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/support-requests/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupportRequest>> GetById(Guid id)
        {
            var item = await _context.SupportRequests.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}

