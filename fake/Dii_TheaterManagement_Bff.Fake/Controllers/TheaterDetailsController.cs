using FakeTheaterBff.Data;
using FakeTheaterBff.Features.SyntheticBehavior;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTheaterBff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterDetailsController : ControllerBase
    {
        private readonly FakeTheaterBffContext _context;
        private readonly UserInfoAccessor _userInfoAccessor;

        public TheaterDetailsController(FakeTheaterBffContext context, UserInfoAccessor userInfoAccessor)
        {
            _context = context;
            _userInfoAccessor = userInfoAccessor;
        }

        // GET: api/TheaterDetails/5
        [HttpGet]
        public async Task<ActionResult<DetailedTheater>> GetDetailedTheater()
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            var detailedTheater = await _context.DetailedTheaters
                .SingleAsync(t => t.TheaterCode == theaterCode);
            if (detailedTheater == null)
            {
                return NotFound();
            }

            return detailedTheater;
        }

        // PUT: api/TheaterDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutDetailedTheater(DetailedTheater detailedTheater)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            detailedTheater.TheaterCode = theaterCode;

            _context.Entry(detailedTheater).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailedTheaterExists(theaterCode))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool DetailedTheaterExists(string theaterCode)
        {
            return _context.DetailedTheaters.Any(e => e.TheaterCode == theaterCode);
        }
    }
}
