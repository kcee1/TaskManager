using TaskManager.DAL.Data;
using TaskManager.DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DAL.SeviceExtention
{
    public static class ServiceExtension
    {
      


        //public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        //{
        //    app.UseExceptionHandler(error => {
        //        error.Run(async context => {
        //            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        //            context.Response.ContentType = "Application/json";
        //            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        //            if (contextFeature != null)
        //            {
        //                Log.Error($"Something went wrong in the {contextFeature.Error}");

        //                await context.Response.WriteAsync(new Error
        //                {
        //                    StatusCode = context.Response.StatusCode,
        //                    Message = "Internal server error please try again later"
        //                }.ToString());
        //            }
        //        });
        //    });
        //}
    }
}
