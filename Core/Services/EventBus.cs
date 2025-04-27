namespace Core.Services
{
    public class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> subscribers = new();

        public static void Subscribe<T>(Action<T> callback)
        {
            if (!subscribers.ContainsKey(typeof(T)))
                subscribers[typeof(T)] = new List<Delegate>();

            subscribers[typeof(T)].Add(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            if (subscribers.TryGetValue(typeof(T), out var list))
                list.Remove(callback);
        }

        public static void Publish<T>(T eventData)
        {
            if (subscribers.TryGetValue(typeof(T), out var list))
            {
                foreach (var callback in list.Cast<Action<T>>())
                    callback(eventData);
            }
        }
    }
}
