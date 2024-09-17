namespace Bermuda.Core.Cache
{
    internal static class ThrowHelper
    {
        internal static void ThrowIfNull<T>(T value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }
    }
}