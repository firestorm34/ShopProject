using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShopProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(AddAppCongiguration).ConfigureLogging(builder=> builder.AddConsole())
            .ConfigureLogging(builder => builder.AddSeq())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //Setting Kerstrel as main host and IIS as proxy server
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                    
                    webBuilder.UseIIS();
                    webBuilder.UseIISIntegration();
                });
        
        // generating new configuration settings
          public static void AddAppCongiguration(HostBuilderContext hostBuilderContext,
            IConfigurationBuilder config){
            var env = hostBuilderContext.HostingEnvironment;
            config.Sources.Clear();
         // Three settings will applied, the last has higher 
         // priority in setting

            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //.AddJsonFile("sharedSettings.json", optional: true)
            .AddEnvironmentVariables();

            }
        
    }


    
}
