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

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var items = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            if (items.Count <= 0) return NotFound();
            foreach(var o in items)
            { 
                var satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == o.Id).ToList();
                if (satellites != null)
                {
                    o.Satellites = satellites;
                }
            }
            return Ok(items);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cObjects = _context.CelestialObjects.ToList();
            foreach(var c in cObjects)
            {
                var satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == c.Id).ToList();
                if (satellites != null)
                {
                    c.Satellites = satellites;
                }
            }
            return Ok(cObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
                _context.CelestialObjects.Add(celestialObject);
                _context.SaveChanges();
                return CreatedAtRoute("GetById", new { id = celestialObject.Id}, celestialObject);
            
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var obj = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);
            if (obj == null) return NotFound();
            obj.Name = celestialObject.Name;
            obj.OrbitalPeriod = celestialObject.OrbitalPeriod;
            obj.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.Update(obj);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult ReanmeObject(int id, string name)
        {
            var obj = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);
            if (obj == null) return NotFound();
            obj.Name = name;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objList = _context.CelestialObjects
                .Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();
            if (objList.Count <= 0) return NotFound();
            _context.RemoveRange(objList);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
