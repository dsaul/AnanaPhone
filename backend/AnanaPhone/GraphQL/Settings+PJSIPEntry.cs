using AnanaPhone.SettingsManager;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Serilog;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Mutation("pjsipEntryRemove")]
		public GenericReturn PJSIPEntryRemove(string name)
		{
			string status;

			try
			{
				SM.PJSIPWizardRowDeleteAllForName(name);

				status = "success";
			}
			catch (UnauthorizedAccessException e)
			{
				status = e.Message;
				goto done;
			}
			catch (Exception e)
			{
				Log.Error(e, "[{Class}.{Method}()] {Message}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
				throw;
			}

		done:
			return new GenericReturn()
			{
				Status = status,
			};
		}

		[Mutation("pjsipEntryUpsert")]
		public GenericReturn PJSIPEntryUpsert(PJSIPEntry e164, string name, string templateName, bool isTemplate)
		{
			string status;

			try
			{
				e164.ForceRowNamesTo(name);
				e164.ForceUsesTemplateTo(templateName);
				e164.ForceRowTemplateTo(isTemplate);
				SM.PJSIPEntryUpsert(e164);
				status = "success";
			}
			catch (UnauthorizedAccessException e)
			{
				status = e.Message;
				goto done;
			}
			catch (Exception e)
			{
				Log.Error(e, "[{Class}.{Method}()] {Message}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
				throw;
			}

		done:
			return new GenericReturn()
			{
				Status = status,
			};
		}

	}
}
