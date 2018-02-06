using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Owin;
using System.Net.Http.Headers;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.FileSystems;
using System.Reflection;
using TypeScriptHTMLApp;

namespace OWINTest.Service
{
    class Startup
    {
        //  Hack from http://stackoverflow.com/a/17227764/19020 to load controllers in 
        //  another assembly.  Another way to do this is to create a custom assembly resolver
        Type valuesControllerType = typeof(OWINTest.API.ValuesController);

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            
            //  Enable attribute based routing
            //  http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //appBuilder.UseStaticFiles("/static");

            //const string rootFolder = ".";
            //System.Reflection.Assembly ass = ;
            var fileSystem = new EmbeddedResourceFileSystem( Assembly.GetAssembly(typeof(StaticClass)), "TypeScriptHTMLApp");
            var options = new StaticFileOptions
            {
                FileSystem = fileSystem
            };

            //appBuilder.UseFileServer(options);
            //appBuilder.UseDirectoryBrowser();
            appBuilder.UseStaticFiles(options);
            var defOptions = new DefaultFilesOptions
            {
                DefaultFileNames = { "index.html" },
                FileSystem = fileSystem
            };
            appBuilder.UseDefaultFiles(defOptions);
            appBuilder.UseWebApi(config);

        } 
    }
}
