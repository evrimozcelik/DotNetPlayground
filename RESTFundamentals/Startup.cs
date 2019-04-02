using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RESTFundamentals.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace RESTFundamentals
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<SampleDBContext>(opt => opt.UseInMemoryDatabase(databaseName:"sample"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Customer API", Version = "v1" });
                //c.OperationFilter<ServiceHeaderFilter>();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SampleDBContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            Mapper.Initialize(cfg => {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<CustomerEntity, Customer>();
                cfg.CreateMap<Customer, CustomerEntity>();
            });

            // Populate test database
            SeedTestData(dbContext);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void SeedTestData(SampleDBContext dbContext)
        {
            var customerEntityList = new List<CustomerEntity>()
            {
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C001", FirstName = "Ned", LastName = "Stark" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C002", FirstName = "Robert", LastName = "Baratheon" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C003", FirstName = "Jaime", LastName = "Lannister" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C004", FirstName = "Catelyn", LastName = "Stark" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C005", FirstName = "Cersei", LastName = "Lannister" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C006", FirstName = "Daenerys", LastName = "Targaryen" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C007", FirstName = "Jorah", LastName = "Mormont" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C008", FirstName = "Viserys", LastName = "Targaryen" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C009", FirstName = "Jon", LastName = "Snow" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C010", FirstName = "Sansa", LastName = "Stark" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C011", FirstName = "Arya", LastName = "Stark" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C012", FirstName = "Robb", LastName = "Stark" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C013", FirstName = "Theon", LastName = "Greyjoy" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C014", FirstName = "Bran", LastName = "Stark" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C015", FirstName = "Joffrey", LastName = "Baratheon" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C016", FirstName = "Sandor (The Hound)", LastName = "Clegane" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C017", FirstName = "Tyrion", LastName = "Lannister" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C018", FirstName = "Khal", LastName = "Drogo" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C019", FirstName = "Petyr (Littlefinger)", LastName = "Baelish" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C020", FirstName = "Davos", LastName = "Seaworth" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C021", FirstName = "Samwell", LastName = "Tarly" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C022", FirstName = "Stannis", LastName = "Baratheon" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C023", FirstName = "Melisandre", LastName = "" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C024", FirstName = "Jeor", LastName = "Mormont" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C025", FirstName = "Bronn", LastName = "" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C026", FirstName = "Varys", LastName = "" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C027", FirstName = "Shae", LastName = "" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C028", FirstName = "Margaery", LastName = "Tyrell" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C029", FirstName = "Tywin", LastName = "Lannister" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C030", FirstName = "Talisa", LastName = "Maegyr" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C031", FirstName = "Ygritte", LastName = "" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C032", FirstName = "Gendry", LastName = "" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C033", FirstName = "Tormund", LastName = "Giantsbane" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C034", FirstName = "Brienne of Tarth", LastName = "" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C035", FirstName = "Ramsay", LastName = "Bolton" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C036", FirstName = "Gilly", LastName = "" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C037", FirstName = "Daario", LastName = "Naharis" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C038", FirstName = "Missandei", LastName = "" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C039", FirstName = "Ellaria", LastName = "Sand" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C040", FirstName = "Tommen", LastName = "Baratheon" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C041", FirstName = "Jaqen", LastName = "H'ghar" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C042", FirstName = "Roose", LastName = "Bolton" },
                new CustomerEntity { Id = Guid.NewGuid(), CustomerID = "C043", FirstName = "The High Sparrow", LastName = "" }
            };

            dbContext.Customers.AddRange(customerEntityList);

            dbContext.SaveChanges();
        }
    }
}
