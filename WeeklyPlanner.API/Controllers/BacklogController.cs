using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.Infrastructure.Data;

namespace WeeklyPlanner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BacklogController : ControllerBase
{
    private readonly AppDbContext _context;

    public BacklogController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var backlogItems = await _context.BacklogItems
            .Where(b => b.IsActive)
            .ToListAsync();

        return Ok(backlogItems);
    }
}