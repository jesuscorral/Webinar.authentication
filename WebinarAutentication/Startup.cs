using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebinarAutentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // El mecanismo de autenticacion será que tengas o no una cookie para poder iniciar.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();
            services.AddAuthentication(setup => 
            {
                setup.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; // Cuando alguien quier hacer un sign in va a saltar el esquema de autenticacion de OpenIdConnect configurado para usar cookies
                setup.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;  // Utilizado cuando se comprueba si estás autenticado
            })
            .AddOpenIdConnect(setup => 
            {
                // TODO - Pasar esta configuracion a los appSettings
                setup.Authority = "https://demo.identityserver.io"; // Entidad capaz de validar el token
                setup.ClientId = "interactive.public";

                setup.Scope.Clear(); // limpiamos los scopes y añadimos openid y profile para ver qer que los estamos usando, aunque con la linea de "Setup.Scoper.Add("api") ya añade estos dos de forma implicita
                setup.Scope.Add("openid");
                setup.Scope.Add("profile");
                setup.Scope.Add("api");

                setup.ResponseType = "code";
                setup.UsePkce = true;

                setup.SaveTokens = true;
            }).AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
