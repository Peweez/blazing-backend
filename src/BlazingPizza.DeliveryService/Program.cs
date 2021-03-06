using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazingPizza.DeliveryService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddKeyPerFile("/etc/secrets/redis", optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ListenAnyIP(80, o => o.Protocols = HttpProtocols.Http2);
                        options.ListenAnyIP(8080);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
