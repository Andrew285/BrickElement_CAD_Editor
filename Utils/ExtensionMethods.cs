namespace Utils
{
    public static class ExtensionMethods
    {
        public static T With<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
}
