using AnanaPhone.AMI;
using AnanaPhone.Calls;
using AnanaPhone.VoiceMail;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	[GraphRoute("voiceMail")]
	public partial class VoiceMail : GraphController
	{
		VoiceMailManager VMM { get; init; }
		public VoiceMail(VoiceMailManager _VMM)
		{
			VMM = _VMM;
		}
	}
}
