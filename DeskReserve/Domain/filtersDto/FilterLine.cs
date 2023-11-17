namespace DeskReserve.Domain.filtersDto
{
	public enum Operator
	{
		Equals,
		NotEquals,
		GreaterThan,
		LessThan
	};
	public class FilterLine
	{
		public string Field { get; set; }
		
	}
}
