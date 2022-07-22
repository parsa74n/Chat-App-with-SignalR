using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRCore.Chat.Mvc.Data;
using SignalRCore.Chat.Mvc.Models;

[assembly: HostingStartup(typeof(SignalRCore.Chat.Mvc.Areas.Identity.IdentityHostingStartup))]
namespace SignalRCore.Chat.Mvc.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}