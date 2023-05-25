using AnanaPhone.AMI;
using AnanaPhone.Conferences;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Serilog;

namespace AnanaPhone.GraphQL
{
	[GraphRoute("confBridge")]
	public partial class ConfBridge : GraphController
	{
		ConfBridgeManager CBM { get; init; }
		AMIManager AMI { get; init; }

		public ConfBridge(ConfBridgeManager _CBM, AMIManager _AMI)
		{
			CBM = _CBM;
			AMI = _AMI;
		}

	}
}
