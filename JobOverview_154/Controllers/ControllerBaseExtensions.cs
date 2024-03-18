using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobOverview_v154.Data;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using JobOverview_v154.Services;

namespace JobOverview_v154.Controllers
{
	public static class ControllerBaseExtensions
	{
		// Renvoie une réponse HTTP personnalisée pour les erreurs
		public static ActionResult CustomResponseForError(this ControllerBase controller, Exception e)
		{
			if (e is ValidationRulesException vre)
			{
				ValidationProblemDetails vpd = new(vre.Errors);
				return controller.ValidationProblem(vpd);
			}
			else
			{
				ProblemDetails pb = e.ConvertToProblemDetails();
				return controller.Problem(pb.Detail, null, pb.Status, pb.Title);
			}
		}

		// Journalise une erreur avec le détail de l'action et de l'entité concernées,
		// puis renvoie une réponse HTTP personnalisée
		public static ActionResult CustomResponseForError<T>(this ControllerBase controller,
			Exception e, T entity, ILogger logger, [CallerMemberName] string? action = null)
		{
			if (e is not ValidationRulesException)
			{
				ProblemDetails pb = e.ConvertToProblemDetails();
				logger.LogWarning("Action {action}, entité de type {type}\n{détail}\n{entity}",
					action,
					entity?.GetType().Name,
					pb.Detail,
					JsonSerializer.Serialize(entity, new JsonSerializerOptions { WriteIndented = true }));

				return controller.Problem(pb.Detail, null, pb.Status, pb.Title);
			}

			return CustomResponseForError(controller, e);
		}
	}
}
