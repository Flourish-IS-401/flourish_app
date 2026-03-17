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
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly FlourishDbContext _context;

        public UsersController(FlourishDbContext context)
        {
            _context = context;
        }

        // GET /api/users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var items = await _context.Users
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            var item = await _context.Users.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}

