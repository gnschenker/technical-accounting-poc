using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TechnicalAccounting.Infrastructure
{
    public static class RedirectToWhenAsync
    {
        static readonly MethodInfo InternalPreserveStackTraceMethod =
            typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
 
        static class Cache
        {
            // ReSharper disable StaticFieldInGenericType
            private static Dictionary<Type, IDictionary<Type, MethodInfo>> dict
                = new Dictionary<Type, IDictionary<Type, MethodInfo>>();

            public static IDictionary<Type, MethodInfo> GetOrCreateDict(Type type) 
            { 
                if(!dict.ContainsKey(type))
                {
                    var methods = type
                        .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(m => m.Name == "When")
                        .Where(m => m.GetParameters().Length == 1)
                        .ToDictionary(m => m.GetParameters().First().ParameterType, m => m);
                    dict.Add(type, methods);
                }
                return dict[type];
            }
            // ReSharper restore StaticFieldInGenericType
        }
 
        [DebuggerNonUserCode]
        public static async Task InvokeEventOptional(object instance, object @event)
        {
            MethodInfo info;
            var type = @event.GetType();
            if (!Cache.GetOrCreateDict(instance.GetType()).TryGetValue(type, out info))
            {
                // we don't care if state does not consume events
                // they are persisted anyway
                return;
            }
            try
            {
                await (Task)info.Invoke(instance, new[] { @event });
            }
            catch (TargetInvocationException ex)
            {
                if (null != InternalPreserveStackTraceMethod)
                    InternalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);
                throw ex.InnerException;
            }
        }
    }
}