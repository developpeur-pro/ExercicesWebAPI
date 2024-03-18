
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
			// R�cup�re la cha�ne de connexion � la base dans les param�tres
			string? connect = builder.Configuration.GetConnectionString("JobOverviewConnect");

			// Enregistre la classe de contexte de donn�es comme service
			// en lui indiquant la connexion � utiliser
			builder.Services.AddDbContext<ContexteJobOverview>(
				opt => opt.UseSqlServer(connect).EnableSensitiveDataLogging());

			builder.Services.AddScoped<IServiceLogiciels, ServiceLogiciels>();
			builder.Services.AddScoped<IServiceEquipes, ServiceEquipes>();
			builder.Services.AddScoped<IServiceTaches, ServiceTaches>();

			builder.Services.AddControllers().AddNewtonsoftJson();

			//builder.Services.AddEndpointsApiExplorer();
			//builder.Services.AddSwaggerGen();

			// G�n�rateur de description d'API NSwag
			builder.Services.AddOpenApiDocument(options =>
			{
				options.Title = "API JobOverview";
				options.Description = "<strong>API JobOverview pour formation ASP.Net Core.<br/>Code dispo sur <a href='https://github.com/developpeur-pro/ExercicesWebAPI'>ce r�f�rentiel GitHub</a></strong>";
			});

			// Ajoute le service d'authentification par porteur de jetons JWT
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					// url d'acc�s au serveur d'identit� 
					options.Authority = builder.Configuration["IdentityServerUrl"];
					options.TokenValidationParameters.ValidateAudience = false;

					// Tol�rance sur la dur�e de validit� du jeton
					options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
				});

			// Ajoute le service d'autorisation
			builder.Services.AddAuthorization(options =>
			{
				// Sp�cifie que tout utilisateur de l'API doit �tre authentifi�
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();

				// Seuls les managers peuvent g�rer les t�ches
				options.AddPolicy("G�rerTaches", p => p.RequireClaim("manager"));

				// Seuls les chefs de service peuvent g�rer les �quipes et personnes
				options.AddPolicy("G�rerEquipes", p => p.RequireClaim("m�tier", "CDS"));
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				// Middleware serveur de d�finition d'API
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