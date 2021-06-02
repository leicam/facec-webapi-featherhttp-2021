using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Facec.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = WebApplication.Create(args);

            app.MapPost("/api/v1/aluno", AdicionarAluno);

            await app.RunAsync();
        }

        private static async Task AdicionarAluno(HttpContext httpContext)
        {
            try
            {
                using (var dataBaseContext = new DataBaseContext())
                {
                    var aluno = await httpContext.Request.ReadJsonAsync<Aluno>();
                    await dataBaseContext.AddAsync(aluno);
                    await dataBaseContext.SaveChangesAsync();

                    httpContext.Response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}