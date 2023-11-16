using DeskReserve.Utils;

namespace DeskReserve.Domain
{
    public class RequestDto
    {
        public DateTime? RequestStartDate { get; set; }

        public DateTime? RequestEndDate { get; set; }

        public BookingState State { get; set; } = BookingState.Requested;

        public bool isPermanentlyOccupied { get; set; }

        public Guid DeskId { get; set; }
    }
}
