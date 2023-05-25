using AnanaPhone.AMI;
using AnanaPhone.Calls;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	[GraphRoute("calls")]
	public partial class Calls : GraphController
	{
		ActiveCallManager ACM { get; init; }
		AMIManager AMI { get; init; }
		HistoricCallManager CHM { get; init; }

		public Calls(ActiveCallManager _ACM, AMIManager _AMI, HistoricCallManager _CHM)
		{
			ACM = _ACM;
			AMI = _AMI;
			CHM = _CHM;
		}

	}
}
