using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Utils
{
	public class RequestValidation : IValidatableObject
	{
		private readonly Request request;
		private readonly BookingState newState;

		public RequestValidation(Request request, BookingState newState)
		{
			this.request = request;
			this.newState = newState;
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validationResults = new List<ValidationResult>();

			if (request.OccupationStatus == OccupationStatus.Permanently)
			{
				request.RequestStartDate = DateTime.MinValue;
				request.RequestEndDate = DateTime.MinValue;

				if (request.RequestStartDate != DateTime.MinValue || request.RequestEndDate != DateTime.MinValue)
				{
					validationResults.Add(new ValidationResult("For permanently occupied requests, BookingStartDate and BookingEndDate must be DateTime.MinValue.", new[] { nameof(request.RequestStartDate), nameof(request.RequestEndDate) }));
				}
			}

			if (request.State != BookingState.Requested)
			{
				validationResults.Add(new ValidationResult("Invalid state update. State can only be updated for requests with state Requested.", new[] { nameof(request.State) }));
			}

			if (newState != BookingState.Approved && newState != BookingState.Rejected)
			{
				validationResults.Add(new ValidationResult("Invalid state update. State can only be updated to Approved or Rejected.", new[] { nameof(newState) }));
			}

			return validationResults;
		}
	}
}
