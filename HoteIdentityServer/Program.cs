using Duende.IdentityServer.Models;
using HoteIdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

// Ajoute et configure le service IdentityServer
builder.Services.AddIdentityServer(options =>
      options.Authentication.CoordinateClientLifetimesWithUserSession = true)

    // Crée des identités
    .AddInMemoryIdentityResources(new IdentityResource[] {
         new IdentityResources.OpenId(),
         new IdentityResources.Profile(),
    })

    // Configure une appli cliente
    .AddInMemoryClients(new Client[] {
         new Client
         {
            ClientId = "BlazorJO",
            ClientSecrets = { new Secret("$xcuk%7yRLiKqD#r".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            
            // Urls auxquelles envoyer les jetons
            RedirectUris = { "https://localhost:7189/signin-oidc" },
            // Urls de redirection aprés déconnexion
            PostLogoutRedirectUris = { "https://localhost:7189/signout-callback-oidc" },
            // Url pour envoyer une demande de déconnexion au serveur d'identité
            FrontChannelLogoutUri = "https://localhost:7189/signout-oidc",

				// Etendues d'API autorisées
				AllowedScopes = { "openid", "profile" },

            // Autorise le client à utiliser un jeton d'actualisation
				AllowOfflineAccess = true,
			}
    })
    // Indique d'utiliser ASP.Net Core Identity pour la gestion des profils et revendications
    .AddAspNetIdentity<IdentityUser>();

// Ajoute la journalisation au niveau debug des événements émis par Duende
builder.Services.AddLogging(options =>
{
   options.AddFilter("Duende", LogLevel.Debug);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseMigrationsEndPoint();
}
else
{
   app.UseExceptionHandler("/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ajoute le middleware d'authentification avec IdentityServer dans le pipeline
app.UseIdentityServer();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
