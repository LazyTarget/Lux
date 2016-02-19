namespace Lux.Extensions
{
    public static class LanguageExtensions
    {
        public static TResult CastTo<TResult>(this object obj)
        {
            var result = (TResult) obj;
            return result;
        }

        public static TResult CastAs<TResult>(this object obj)
            where TResult : class
        {
            var result = obj as TResult;
            return result;
        }

    }
}
