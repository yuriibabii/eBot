using System;

namespace eBot.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static TType Resolve<TType>(this IServiceProvider serviceProvider)
        {
            var instance = serviceProvider.GetService(typeof(TType));
            return (TType)instance;
        }
    }
}