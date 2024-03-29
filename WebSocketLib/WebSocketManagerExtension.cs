﻿// Ahmet CAN 
// http://ahmetcan.com.tr 
// eposta@ahmetcan.com.tr

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SohbetServer.WebSocketLib
{
    public static class WebSocketManagerExtensions
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services, Assembly assembly = null)
        {
            services.AddTransient<WebSocketConnectionManager>();

            Assembly ass = assembly ?? Assembly.GetEntryAssembly();

            foreach (var type in ass.ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }

        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                              PathString path,
                                                              WebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }
    }
}