﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
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
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
            return Ok(celestialObject);
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name);
            if (!celestialObjects.Any())
                return NotFound();
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
           var matchedId=_context.CelestialObjects.Find(id);
            if (matchedId==null)
                return NotFound();
            matchedId.Name = celestialObject.Name;
            matchedId.OrbitalPeriod = celestialObject.OrbitalPeriod;
            matchedId.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(matchedId);
            _context.SaveChanges();
            return NoContent();
           
        }
        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var matchedId = _context.CelestialObjects.Find(id);
            if (matchedId == null)
                return NotFound();
            matchedId.Name = name;
            _context.CelestialObjects.Update(matchedId);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var matchedId = _context.CelestialObjects.Where(e=>e.Id==id||e.OrbitedObjectId==id);
            if (!matchedId.Any())
                return NotFound();
            _context.CelestialObjects.RemoveRange(matchedId);
            _context.SaveChanges();
            return NoContent();
           
        }
    }
}
