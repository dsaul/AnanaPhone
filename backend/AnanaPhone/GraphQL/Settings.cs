﻿using AnanaPhone.AMI;
using AnanaPhone.Boot;
using AnanaPhone.Conferences;
using AnanaPhone.SettingsManager;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Serilog;
using System.Threading.Channels;

namespace AnanaPhone.GraphQL
{
	[GraphRoute("settings")]
	public partial class Settings : GraphController
	{
		ConfBridgeManager CBM { get; init; }
		AMIManager AMI { get; init; }
		SettingsManager.Manager SM { get; init; }
		BootManager BM { get; init; }

		public Settings(
			ConfBridgeManager _CBM, 
			AMIManager _AMI, 
			SettingsManager.Manager _SM,
			BootManager _BM
			)
		{
			CBM = _CBM;
			AMI = _AMI;
			SM = _SM;
			BM = _BM;
		}

	}
}
