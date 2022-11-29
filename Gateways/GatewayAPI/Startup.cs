    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Ocelot.DependencyInjection;
    using Ocelot.Middleware;
    using Swashbuckle;

    namespace GatewayAPI
    {
        public class Startup
        {
        // This method gets called by the runtime. Use this method to add services to the container.
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        } // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            
        public void ConfigureServices(IServiceCollection services)
            {


            services.AddOcelot();
            // services.AddSwaggerForOcelot(_configuration);
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});
        }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }


                app.UseRouting();



                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync("Hello World!");
                    });
                });

              
            //  app.UseSwaggerForOcelotUI(opt =>
            //{
            //    opt.PathToSwaggerGenerator = "/swagger/docs";
            //});
            await app.UseOcelot();

        }
    }
    }


