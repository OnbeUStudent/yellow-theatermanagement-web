using FakeTheaterBff.Data;
using FakeTheaterBff.Features.SyntheticBehavior;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTheaterBff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnackPackOrdersController : ControllerBase
    {
        private readonly FakeTheaterBffContext _context;
        private readonly UserInfoAccessor _userInfoAccessor;

        public SnackPackOrdersController(FakeTheaterBffContext context, UserInfoAccessor userInfoAccessor)
        {
            _context = context;
            _userInfoAccessor = userInfoAccessor;
        }

        // GET: api/SnackPackOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SnackPackOrder>>> GetSnackPackOrders()
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            return await _context.SnackPackOrders
                .AsNoTracking()
                .Where(spo => spo.DetailedTheater.TheaterCode == theaterCode)
                .ToListAsync();
        }

        // GET: api/SnackPackOrders/5
        [HttpGet("{monthId}")]
        public async Task<ActionResult<SnackPackOrder>> GetSnackPackOrder(long monthId)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            var snackPackOrder = await _context.SnackPackOrders
                .AsNoTracking()
                .Where(spo => spo.TheaterCode == theaterCode)
                .SingleOrDefaultAsync(spo => spo.MonthId == monthId);

            if (snackPackOrder == null)
            {
                return NotFound();
            }

            return snackPackOrder;
        }

        // PUT: api/SnackPackOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutSnackPackOrder(int monthId, SnackPackOrder snackPackOrder)
        {
            if (monthId != snackPackOrder.MonthId)
            {
                return BadRequest();
            }
            string theaterCode = _userInfoAccessor.TheaterCode;
            snackPackOrder.TheaterCode = theaterCode;

            try
            {
                var oldSnackPackOrder = _context.SnackPackOrders.SingleOrDefault(b => b.MonthId == monthId && b.TheaterCode == theaterCode);
                if (oldSnackPackOrder != null)
                {
                    _context.SnackPackOrders.Remove(oldSnackPackOrder);
                }
                _context.SnackPackOrders.Add(snackPackOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SnackPackOrderExists(theaterCode, monthId))
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

        // DELETE: api/SnackPackOrders/5
        [HttpDelete("{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteSnackPackOrder(int monthId)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            var snackPackOrder = await _context.SnackPackOrders
                .Where(spo => spo.TheaterCode == theaterCode)
                .SingleOrDefaultAsync(b => b.MonthId == monthId);
            if (snackPackOrder == null)
            {
                return NotFound();
            }
            snackPackOrder.TheaterCode = theaterCode;

            _context.SnackPackOrders.Remove(snackPackOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SnackPackOrderExists(string theaterCode, int monthId)
        {
            return _context.SnackPackOrders
                .Any(e => e.TheaterCode == theaterCode && e.MonthId == monthId);
        }
    }
}
