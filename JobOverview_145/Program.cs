
using Asp.Versioning;
using JobOverview_v145.Data;
using JobOverview_v145.Services;
using JobOverview_v145.V2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;

namespace JobOverview_v145
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			// Récupère la chaîne de connexion à la base dans les paramètres
			string? connect = builder.Configuration.GetConnectionString("JobOverviewConnect");

			// Enregistre la classe de contexte de données comme service
			// en lui indiquant la connexion à utiliser
			builder.Services.AddDbContext<ContexteJobOverview>(
				opt => opt.UseSqlServer(connect).EnableSensitiveDataLogging());

			builder.Services.AddScoped<IServiceLogiciels, ServiceLogiciels>();
			builder.Services.AddScoped<IServiceEquipes, ServiceEquipes>();
			builder.Services.AddScoped<V1.Services.IServiceEquipes, V1.Services.ServiceEquipes>();
			builder.Services.AddScoped<IServiceTaches, ServiceTaches>();

			builder.Services.AddControllers().AddNewtonsoftJson();

			// Ajoute le service d'authentification par porteur de jetons JWT
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					// url d'accès au serveur d'identité 
					options.Authority = builder.Configuration["IdentityServerUrl"];
					options.TokenValidationParameters.ValidateAudience = false;

					// Tolérance sur la durée de validité du jeton
					options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
				});

			// Ajoute le service d'autorisation
			builder.Services.AddAuthorization(options =>
			{
				// Spécifie que tout utilisateur de l'API doit être authentifié
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();

				// Seuls les managers peuvent gérer les tâches
				options.AddPolicy("GérerTaches", p => p.RequireClaim("manager"));

				// Seuls les chefs de service peuvent gérer les équipes et personnes
				options.AddPolicy("GérerEquipes", p => p.RequireClaim("métier", "CDS"));
			});

			// Enregistre le service de versionnage
			builder.Services.AddApiVersioning(options => {
				options.ApiVersionReader = ApiVersionReader.Combine(
					new QueryStringApiVersionReader(),
					new HeaderApiVersionReader("x-api-version"));
				options.AssumeDefaultVersionWhenUnspecified = true;
			})
			.AddMvc()
			.AddApiExplorer(options =>
			{
				options.GroupNameFormat = "'v'VVVV"; // format du numéro de version
				options.SubstituteApiVersionInUrl = true;
			});

			// Définit les numéros de version
			var versions = new[] { new ApiVersion(1.0), new ApiVersion(2.0) };

			// Crée les docs de définitions d'API
			foreach (ApiVersion vers in versions)
			{
				builder.Services.AddOpenApiDocument(options =>
				{
					string version = vers.ToString("'v'VVVV");
					options.DocumentName = version;
					options.ApiGroupNames = new[] { version };
					options.Title = "API JobOverview";
					options.Description = "<strong>API JobOverview pour formation ASP.Net Core.<br/>Code dispo sur <a href='https://github.com/developpeur-pro/ExercicesWebAPI'>ce référentiel GitHub</a></strong>";
					options.Version = version;
				});
			}

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				// Middleware serveur de définition d'API
				app.UseOpenApi();

				// Interface web pour la doc
				app.UseSwaggerUi(options =>
				{
					foreach (ApiVersion vers in versions)
					{
						string version = vers.ToString("'v'VVVV");
						var route = new SwaggerUiRoute(version, $"/swagger/{version}/swagger.json");
						options.SwaggerRoutes.Add(route);
					}
				});
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			var endpointBuilder = app.MapControllers();
			if (app.Environment.IsDevelopment())
				endpointBuilder.AllowAnonymous();

			app.Run();
		}
	}
}