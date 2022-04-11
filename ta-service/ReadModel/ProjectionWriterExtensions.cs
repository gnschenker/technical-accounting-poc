using System;
using System.Threading.Tasks;

namespace TechnicalAccounting.ReadModel
{
    public static class ProjectionWriterExtensions
    {
        public static async Task Add<TView>(this IProjectionWriter<TView> writer, string key, TView item) where TView : class
        {
            await writer.AddOrUpdate(key, () => item, null, false);
        }

        public static async Task Add<TView>(this IProjectionWriter<TView> writer, string key, Func<TView> addFactory) where TView : class
        {
            await writer.AddOrUpdate(key, addFactory, null, false);
        }

        public static async Task Update<TView>(this IProjectionWriter<TView> writer, string key, Action<TView> update) where TView : class
        {
            await writer.AddOrUpdate(key, null, x => { update(x); return x; });
        }

        public static async Task Update<TView>(this IProjectionWriter<TView> writer, string key, Func<TView, TView> update) where TView : class
        {
            await writer.AddOrUpdate(key, null, update);
        }

        public static async Task UpdateEnforcingNew<TView>(this IProjectionWriter<TView> writer, string key, Action<TView> update) 
            where TView : class, new()
        {
            await writer.AddOrUpdate(key, () => new TView(), x => { update(x); return x; });
        }
    }
}