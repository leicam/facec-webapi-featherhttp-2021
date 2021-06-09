using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
            app.MapPost("/api/v1/aluno/{registroAcademico}", AlterarAluno);
            app.MapGet("/api/v1/aluno", BuscarTodos);
            app.MapGet("/api/v1/aluno/{registroAcademico}", BuscarAluno);
            app.MapDelete("/api/v1/aluno", DeletarTodos);
            app.MapDelete("/api/v1/aluno/{registroAcademico}", DeletarAluno);

            await app.RunAsync();
        }

        #region AdicionarAluno
        private static async Task AdicionarAluno(HttpContext httpContext)
        {
            try
            {
                using (var dataBaseContext = new DataBaseContext())
                {
                    var aluno = await httpContext.Request.ReadJsonAsync<Aluno>();
                    await dataBaseContext.Alunos.AddAsync(aluno);
                    await dataBaseContext.SaveChangesAsync();

                    httpContext.Response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion AdicionarAluno

        #region AlterarAluno
        private static async Task AlterarAluno(HttpContext httpContext)
        {
            if(!httpContext.Request.RouteValues.TryGet("registroAcademico", out string registroAcademico))
            {
                httpContext.Response.StatusCode = 400;
                return;
            }

            try
            {
                using (var dataBaseContext = new DataBaseContext())
                {
                    var aluno = await dataBaseContext.Alunos.FindAsync(registroAcademico);

                    if (aluno == null)
                    {
                        httpContext.Response.StatusCode = 400;
                        return;
                    }

                    var alunoRequest = await httpContext.Request.ReadJsonAsync<Aluno>();

                    aluno.Nome = alunoRequest.Nome;

                    await dataBaseContext.SaveChangesAsync();

                    httpContext.Response.StatusCode = 204;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion AlterarAluno

        #region BuscarTodos
        private static async Task BuscarTodos(HttpContext httpContext)
        {
            try
            {
                using (var dataBaseContext = new DataBaseContext())
                {
                    await httpContext.Response.WriteJsonAsync(await dataBaseContext.Alunos.ToListAsync());
                    httpContext.Response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion BuscarTodos

        #region BuscarAluno
        private static async Task BuscarAluno(HttpContext httpContext)
        {
            if(!httpContext.Request.RouteValues.TryGet("registroAcademico", out string registroAcademico))
            {
                httpContext.Response.StatusCode = 400;
                return;
            }

            try
            {
                using (var dataBaseContext = new DataBaseContext())
                {
                    await httpContext.Response
                        .WriteJsonAsync(await dataBaseContext.Alunos.FindAsync(registroAcademico) ?? new Aluno());
                    httpContext.Response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion BuscarAluno

        #region DeletarTodos
        public static async Task DeletarTodos(HttpContext httpContext)
        {
            try
            {
                using (var dataBaseContext = new DataBaseContext())
                {
                    dataBaseContext.Alunos.RemoveRange(dataBaseContext.Alunos);
                    await dataBaseContext.SaveChangesAsync();

                    httpContext.Response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync(ex.Message);
            }
        }
        #endregion DeletarTodos

        #region DeletarAluno
        private static async Task DeletarAluno(HttpContext httpContext)
        {
            try
            {
                if (!httpContext.Request.RouteValues.TryGet("registroAcademico", out string registroAcademico))
                    throw new ArgumentException(nameof(registroAcademico));

                using (var dataBaseContext = new DataBaseContext())
                {
                    var aluno = await dataBaseContext.Alunos.FindAsync(registroAcademico)
                        ?? throw new NullReferenceException(nameof(Aluno));

                    dataBaseContext.Alunos.Remove(aluno);
                    await dataBaseContext.SaveChangesAsync();

                    httpContext.Response.StatusCode = 200;
                }
            }
            catch (ArgumentException ex)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                httpContext.Response.StatusCode = 404;
                await httpContext.Response.WriteAsync(ex.Message);
            }
        }
        #endregion DeletarAluno
    }
}