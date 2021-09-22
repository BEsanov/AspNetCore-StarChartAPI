using System;
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

        public CelestialObjectController(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        [HttpGet("{id:int}",Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var matchedClSObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (matchedClSObject == null)
            {
                return NotFound();
            }
            var takeSateliteProperty = matchedClSObject.Satellites.FirstOrDefault(c => c.OrbitedObjectId == id);
            if (takeSateliteProperty != null)
            {
                return Ok();
            }
            return NotFound();
        }
        //[HttpGet("{name}")]
        //public IActionResult GetByName(string name)
        //{
        //    var matchedClSObject = _context.CelestialObjects.FirstOrDefault(c => c.Name == name);
        //    if (matchedClSObject == null)
        //    {
        //        return NotFound();
        //    }
        //    foreach (var it in matchedClSObject)
        //    {

        //    }
        //}
    }
}
