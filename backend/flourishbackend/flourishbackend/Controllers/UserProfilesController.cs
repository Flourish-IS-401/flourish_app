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
    [Route("api/user-profiles")]
    public class UserProfilesController : ControllerBase
    {
        private readonly FlourishDbContext _context;

        public UserProfilesController(FlourishDbContext context)
        {
            _context = context;
        }

        // GET /api/user-profiles
        [HttpGet]
        public async Task<ActionResult<List<UserProfile>>> GetAll()
        {
            var items = await _context.UserProfiles
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/user-profiles/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserProfile>> GetById(Guid id)
        {
            var item = await _context.UserProfiles.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}

