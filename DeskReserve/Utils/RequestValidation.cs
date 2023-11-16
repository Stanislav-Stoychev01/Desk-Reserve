using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DeskReserve.Data;

namespace DeskReserve.Utils
{
	public class RequestValidation : IValidatableObject
	{
		private readonly bool isPremanentlyOccuoied;
		private readonly DateTime bookingStartDate;
		private readonly DateTime bookingEndDate;

		public RequestValidation(bool isPremanentlyOccuoied, DateTime bookingStartDate, DateTime bookingEndDate)
		{
			this.isPremanentlyOccuoied = isPremanentlyOccuoied;
			this.bookingStartDate = bookingStartDate;
			this.bookingEndDate = bookingEndDate;
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (isPremanentlyOccuoied && (bookingStartDate != DateTime.MinValue || bookingEndDate != DateTime.MinValue))
			{
				yield return new ValidationResult("For permanently occupied requests, BookingStartDate and BookingEndDate must be DateTime.MinValue.", new[] { nameof(bookingStartDate), nameof(bookingEndDate) });
			}
		}
	}
}
