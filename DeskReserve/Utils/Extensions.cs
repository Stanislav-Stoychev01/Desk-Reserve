namespace DeskReserve.Utils
{
    public static class Extensions
    {
        public static void MapProperties<TSource, TDestination>(this TSource source, TDestination destination)
                where TSource : class, new()
                where TDestination : class, new()
        {
            if (source != null && destination != null)
            {
                var sourceProperties = source.GetType().GetProperties();
                var destinationProperties = destination.GetType().GetProperties();

                foreach (var sourceProperty in sourceProperties)
                {
                    var destinationProperty = destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);
                    if (destinationProperty != null && destinationProperty.PropertyType == sourceProperty.PropertyType)
                    {
                        destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                    }
                }
            }
        }
    }
}