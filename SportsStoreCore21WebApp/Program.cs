﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Logging;

namespace SportsStoreCore21WebApp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseApplicationInsights()
            .ConfigureAppConfiguration((hostingContext, config) => {
              if (!hostingContext.HostingEnvironment.IsDevelopment())
              {
                SetupKeyVault(hostingContext, config);
              }
              if (hostingContext.HostingEnvironment.IsDevelopment())
              {
                SetupKeyVault(hostingContext, config);
              }
            })
            .UseStartup<Startup>();

    private static void SetupKeyVault(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
    {
      var buildConfig = config.Build();
      var keyVaultEndpoint = buildConfig["SportsStoreKeyVault"];
      if (!string.IsNullOrEmpty(keyVaultEndpoint))
      {
        var azureServiceTokenProvider = new AzureServiceTokenProvider();
        var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
      }
    }
  }
}
