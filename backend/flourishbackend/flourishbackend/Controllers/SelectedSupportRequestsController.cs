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
    [Route("api/selected-support-requests")]
    public class SelectedSupportRequestsController : ControllerBase
    {
        private readonly FlourishDbContext _context;

        public SelectedSupportRequestsController(FlourishDbContext context)
        {
            _context = context;
        }

        // GET /api/selected-support-requests?selectedDate=YYYY-MM-DD
        [HttpGet]
        public async Task<ActionResult<List<SelectedSupportRequest>>> GetAll([FromQuery] string? selectedDate = null)
        {
            IQueryable<SelectedSupportRequest> q = _context.SelectedSupportRequests;

            if (!string.IsNullOrWhiteSpace(selectedDate))
            {
                q = q.Where(x => x.SelectedDate == selectedDate);
            }

            var items = await q
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/selected-support-requests/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SelectedSupportRequest>> GetById(Guid id)
        {
            var item = await _context.SelectedSupportRequests.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}

