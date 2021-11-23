using System.Text;
using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProAgil.Domain.Identity;
using ProAgil.Repository;
using ProAgil.Repository.Data;
using ProAgil.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ProAgil.WebAPI
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
            services.AddDbContext<ProAgilContext>(
                x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
            );
            
            // configuração da senha usuário
            IdentityBuilder builder = services.AddIdentityCore<User>(options =>
                {
                    options.Password.RequireDigit = false; // Remove a obrigatoriedade de caracteres especiais na senha
                    options.Password.RequireNonAlphanumeric = false; // Remove a obrigatoriedade de ter numeros e letras na senha
                    options.Password.RequireLowercase = false; // Remove a obrigatoriedade de caracteres minusculos na senha
                    options.Password.RequireUppercase = false; // Remove a obrigatoriedade de caracteres maiusculos na senha
                    options.Password.RequiredLength = 6; // Informa que quantidade de caracteres na senha
                }
            );

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services); // link da configuração do usuário com o Role
            builder.AddEntityFrameworkStores<ProAgilContext>(); // Configura o identity a identificar o contexto da aplicação
            builder.AddRoleValidator<RoleValidator<Role>>(); // configura quem vai validar os papeis (Classe Role criada no dominio)
            builder.AddRoleManager<RoleManager<Role>>(); //Configura qual classe vai gerenciar os Papeis (classe Role)
            builder.AddSignInManager<SignInManager<User>>(); // Configura quem vai ser usado paa gerenciar o login (Classe User)

            //Configuração JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters{
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddMvc( options => {

                // Configura a politica de autenticação para que sempre valide se existe um usuário autenticado ao acessar uma rota da aplicação
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = 
                                Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<IProAgilRepository, ProAgilRepository>();
            services.AddAutoMapper();
            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

          //  app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions(){
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            } );
            app.UseMvc();
        }
    }
}
