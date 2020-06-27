using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace cw6.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                string querystring = httpContext.Request?.QueryString.ToString();
                string metoda = httpContext.Request.Method.ToString();
                string bodyStr = "";

                using (StreamReader reader
                 = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }

                //logowanie do pliku
                string pathToLogFile = "/home/Code/log.txt";
                if (!File.Exists(pathToLogFile))
                {
                    File.Create(pathToLogFile);
                    TextWriter tw = new StreamWriter(pathToLogFile);
                    tw.WriteLine(bodyStr);
                    tw.Close();
                }
                else if (File.Exists(pathToLogFile))
                {
                    using (var tw = new StreamWriter(pathToLogFile, true))
                    {
                        tw.WriteLine(bodyStr);
                    }
                }
            }

            await _next(httpContext);
        }


    }
}
