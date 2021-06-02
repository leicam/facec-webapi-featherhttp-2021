using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facec.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = WebApplication.Create(args);

            await app.RunAsync();
        }
    }
}