namespace ShowPass.Models
{
    public enum Type
    {
        Camarote,
        Pista
    }

    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, decimal> _prices = new Dictionary<Type, decimal>
        {
            { Type.Camarote, 500.00m },
            { Type.Pista, 300.00m }
        };

        public static decimal GetPrice(this Type type)
        {
            return _prices[type];
        }
    }
}