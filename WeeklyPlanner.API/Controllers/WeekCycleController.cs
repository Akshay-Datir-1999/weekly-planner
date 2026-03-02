using Microsoft.AspNetCore.Mvc;
using WeeklyPlanner.Core.Models;
using WeeklyPlanner.Infrastructure.Data;

namespace WeeklyPlanner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeekCycleController : ControllerBase
{
    private readonly AppDbContext _context;

    public WeekCycleController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWeek([FromBody] WeekCycle request)
    {
        var totalPercent = request.Category1Percent +
                           request.Category2Percent +
                           request.Category3Percent;

        if (totalPercent != 100)
        {
            return BadRequest("Total percentage must equal 100.");
        }

        _context.WeekCycles.Add(request);
        await _context.SaveChangesAsync();

        return Ok(request);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.WeekCycles.ToList());
    }
}