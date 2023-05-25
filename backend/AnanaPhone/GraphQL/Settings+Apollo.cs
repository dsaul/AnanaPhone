using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		// This is due to apollo complaining, it should only change if the app is relaunched.
		static readonly Guid apolloId = Guid.NewGuid();
		[Query("id")]
		public string GetId()
		{
			return apolloId.ToString();
		}
	}
}
