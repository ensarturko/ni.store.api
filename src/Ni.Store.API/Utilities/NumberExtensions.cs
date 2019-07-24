namespace Ni.Store.Api.Utilities
{
    internal static class NumberExtensions
    {
        internal static bool IsGreaterThanZero(this int? value)
        {
            if (!value.HasValue)
            {
                return false;
            }

            return IsGreaterThanZero(value.Value);
        }

        internal static bool IsGreaterThanZero(this int value)
        {
            return value > 0;
        }
    }
}
