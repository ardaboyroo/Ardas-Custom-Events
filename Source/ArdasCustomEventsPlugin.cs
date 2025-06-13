using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal class ArdasCustomEventsPlugin : Plugin<Config>
	{
		public static ArdasCustomEventsPlugin Instance { get; private set; }
		public override string Name { get; } = "ArdasCustomEvents";
		public override string Author { get; } = "ardaboyroo";
		public override string Description { get; } = "This plugin introduces custom events and QoL fixes.";
		public override Version Version { get; } = new Version(1, 1, 0, 0);
		public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

		public CustomEventsManager CustomEventsManager { get; private set; }
		public QolManager QolManager { get; private set; }

		public override void Enable()
		{
			Instance = this;
			Logger.Info("Arda's plugin started!");
			CustomEventsManager = new CustomEventsManager();
			QolManager = new QolManager();
		}

		public override void Disable()
		{
			CustomEventsManager = null;
			QolManager = null;
		}
	}
}
