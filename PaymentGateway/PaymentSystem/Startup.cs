using System.Collections.Generic;
using Couchbase;
using Couchbase.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentSystem.Core;
using PaymentSystem.Core.Configuration;
using PaymentSystem.Core.Domain.EntityFramework.Repositories;
using PaymentSystem.Core.Domain.Providers;
using PaymentSystem.Core.Domain.StateManagement;
using PaymentSystem.Infrastructure.Domain.Repositories;

namespace PaymentSystem.Gateway
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
      services.AddMemoryCache();
      services.AddSingleton<ICacheManager, CacheManager>();
      services.AddSingleton<IPaymentRepository, CouchbasePaymentRepository>();
      services.AddSingleton<IBankProvider, BankProvider.BankProvider>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMemoryCache cache,
      ICacheManager cacheManager, IApplicationLifetime applicationLifetime)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseMvc();

      #region Caching settings

      #region Merchants Settings
      
      var merchantsSettings = Configuration.GetSection(Constants.Configuration.MerchantSettings).Get<MerchantSettings[]>();
      cacheManager.Cache = cache;
      cacheManager.SetMerchantsSettings(merchantsSettings);
      #endregion

      #region Bank Api Settings
      var bankSettings = Configuration.GetSection(Constants.Configuration.BankSettings).Get<BankSettings>();
      cacheManager.SetBankSettings(bankSettings);
      #endregion

      #endregion

      var couchDbSettings = Configuration.GetSection(Constants.Configuration.Couchbase).Get<DatabaseSettings>();
      ClusterHelper.Initialize(
        new Couchbase.Configuration.Client.ClientConfiguration
        {
          Servers = new List<System.Uri> { new System.Uri(couchDbSettings.ConnectionString) }
        }, new PasswordAuthenticator(couchDbSettings.UserName, couchDbSettings.Password));
      applicationLifetime.ApplicationStopped.Register(ClusterHelper.Close);
    }
  }
}
