using FormulaeAPI.Messages;
using FormulaeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormulaeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger<BookingsController> _logger;
        private readonly IMessageProducer _messageProducer;

        //InMemory db
        public static readonly List<Booking> _bookings = new();
        public BookingsController(ILogger<BookingsController> logger, IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public IActionResult CreatingBooking(Booking newBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _bookings.Add(newBooking);
            _messageProducer.SendingMessages<Booking>(newBooking);
            return Ok();
        }

    }
}
