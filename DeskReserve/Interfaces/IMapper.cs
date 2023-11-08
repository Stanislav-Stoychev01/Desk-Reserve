namespace DeskReserve.Interfaces
{
    public interface IMapper
    {
        T MapProperties<T>(object source, T destination);
    }
}