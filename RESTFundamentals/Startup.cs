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
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C001", FirstName2 = "Ned", LastName = "Stark", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C002", FirstName2 = "Robert", LastName = "Baratheon", DateOfBirth = new DateTime(2407,2,7) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C003", FirstName2 = "Jaime", LastName = "Lannister", DateOfBirth = new DateTime(2423,6,12) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C004", FirstName2 = "Catelyn", LastName = "Stark", DateOfBirth = new DateTime(2417,8,1) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C005", FirstName2 = "Cersei", LastName = "Lannister", DateOfBirth = new DateTime(2419,10,24) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C006", FirstName2 = "Daenerys", LastName = "Targaryen", DateOfBirth = new DateTime(2430,2,28) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C007", FirstName2 = "Jorah", LastName = "Mormont", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C008", FirstName2 = "Viserys", LastName = "Targaryen", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C009", FirstName2 = "Jon", LastName = "Snow", DateOfBirth = new DateTime(2407,2,7) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C010", FirstName2 = "Sansa", LastName = "Stark", DateOfBirth = new DateTime(2423,6,12) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C011", FirstName2 = "Arya", LastName = "Stark", DateOfBirth = new DateTime(2417,8,1) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C012", FirstName2 = "Robb", LastName = "Stark", DateOfBirth = new DateTime(2419,10,24) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C013", FirstName2 = "Theon", LastName = "Greyjoy", DateOfBirth = new DateTime(2430,2,28) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C014", FirstName2 = "Bran", LastName = "Stark", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C015", FirstName2 = "Joffrey", LastName = "Baratheon", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C016", FirstName2 = "Sandor (The Hound)", LastName = "Clegane", DateOfBirth = new DateTime(2407,2,7) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C017", FirstName2 = "Tyrion", LastName = "Lannister", DateOfBirth = new DateTime(2423,6,12) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C018", FirstName2 = "Khal", LastName = "Drogo", DateOfBirth = new DateTime(2417,8,1) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C019", FirstName2 = "Petyr (Littlefinger)", LastName = "Baelish", DateOfBirth = new DateTime(2419,10,24) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C020", FirstName2 = "Davos", LastName = "Seaworth", DateOfBirth = new DateTime(2430,2,28) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C021", FirstName2 = "Samwell", LastName = "Tarly", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C022", FirstName2 = "Stannis", LastName = "Baratheon", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C023", FirstName2 = "Melisandre", LastName = "", DateOfBirth = new DateTime(2407,2,7) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C024", FirstName2 = "Jeor", LastName = "Mormont", DateOfBirth = new DateTime(2423,6,12) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C025", FirstName2 = "Bronn", LastName = "", DateOfBirth = new DateTime(2417,8,1) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C026", FirstName2 = "Varys", LastName = "", DateOfBirth = new DateTime(2419,10,24) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C027", FirstName2 = "Shae", LastName = "", DateOfBirth = new DateTime(2430,2,28) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C028", FirstName2 = "Margaery", LastName = "Tyrell", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C029", FirstName2 = "Tywin", LastName = "Lannister", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C030", FirstName2 = "Talisa", LastName = "Maegyr", DateOfBirth = new DateTime(2407,2,7) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C031", FirstName2 = "Ygritte", LastName = "", DateOfBirth = new DateTime(2423,6,12) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C032", FirstName2 = "Gendry", LastName = "", DateOfBirth = new DateTime(2417,8,1) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C033", FirstName2 = "Tormund", LastName = "Giantsbane", DateOfBirth = new DateTime(2419,10,24) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C034", FirstName2 = "Brienne of Tarth", LastName = "", DateOfBirth = new DateTime(2430,2,28) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C035", FirstName2 = "Ramsay", LastName = "Bolton", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C036", FirstName2 = "Gilly", LastName = "", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C037", FirstName2 = "Daario", LastName = "Naharis", DateOfBirth = new DateTime(2407,2,7) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C038", FirstName2 = "Missandei", LastName = "", DateOfBirth = new DateTime(2423,6,12) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C039", FirstName2 = "Ellaria", LastName = "Sand", DateOfBirth = new DateTime(2417,8,1) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C040", FirstName2 = "Tommen", LastName = "Baratheon", DateOfBirth = new DateTime(2419,10,24) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C041", FirstName2 = "Jaqen", LastName = "H'ghar", DateOfBirth = new DateTime(2430,2,28) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C042", FirstName2 = "Roose", LastName = "Bolton", DateOfBirth = new DateTime(2417,1,18) },
                new CustomerEntity {Id = Guid.NewGuid(), CustomerID = "C043", FirstName2 = "The High Sparrow", LastName = "", DateOfBirth = new DateTime(2417,8,1) }
            };

            dbContext.Customers.AddRange(customerEntityList);

            dbContext.SaveChanges();
        }
    }
}
