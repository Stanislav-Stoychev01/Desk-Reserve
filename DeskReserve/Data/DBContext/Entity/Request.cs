using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeskReserve.Domain;
using DeskReserve.Utils;

namespace DeskReserve.Data.DBContext.Entity
{
	[Table("Request")]
	public class Request : IValidatableObject
	{
		[Key]
		public Guid RequestId { get; set; }

		[Required]
		public DateTime? RequestStartDate { get; set; }

		[Required]
		public DateTime? RequestEndDate { get; set; }

		[Required]
		public BookingState State { get; set; } = BookingState.Requested;

		[DefaultValue(false)]
		public OccupationStatus OccupationStatus { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[Required]
		public Guid RejectedBy { get; set; }

		[Required]
		public Guid DeskId { get; set; }
		public Desk Desk { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validationResults = new List<ValidationResult>();

			if (OccupationStatus == OccupationStatus.Permanently)
			{
				RequestStartDate = DateTime.MinValue;
				RequestEndDate = DateTime.MinValue;

				if (RequestStartDate != DateTime.MinValue || RequestEndDate != DateTime.MinValue)
				{
					validationResults.Add(new ValidationResult("For permanently occupied requests, RequestStartDate and RequestEndDate must be DateTime.MinValue.", new[] { nameof(RequestStartDate), nameof(RequestEndDate) }));
				}
			}

			if (State != BookingState.Requested)
			{
				validationResults.Add(new ValidationResult("Invalid state update. State can only be updated for requests with state Requested.", new[] { nameof(State) }));
			}

			return validationResults;
		}
	}
}
