
using JobOverview_v158.Data;
using JobOverview_v158.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JobOverview_v158
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
			builder.Services.AddScoped<IServiceTaches, ServiceTaches>();

			builder.Services.AddControllers().AddNewtonsoftJson();

			//builder.Services.AddEndpointsApiExplorer();
			//builder.Services.AddSwaggerGen();

			// Générateur de description d'API NSwag
			builder.Services.AddOpenApiDocument(options =>
			{
				options.Title = "API JobOverview";
				options.Description = "<strong>API JobOverview pour formation ASP.Net Core.<br/>Code dispo sur <a href='https://github.com/developpeur-pro/ExercicesWebAPI'>ce référentiel GitHub</a></strong>";
			});

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

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				// Middleware serveur de définition d'API
				app.UseOpenApi();

				// Interface web pour la doc
				app.UseSwaggerUi();
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