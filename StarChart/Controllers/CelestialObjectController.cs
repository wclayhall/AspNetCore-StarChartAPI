using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var item = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            var satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == item.Id).ToList();
            if (satellites != null)
            {
                item.Satellites = satellites;
            }
            return Ok(item);
        }
    }
}
