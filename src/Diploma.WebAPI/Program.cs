using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Diploma.WebAPI
{
    public class Program
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }

        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .Run();
        }
    }
}
