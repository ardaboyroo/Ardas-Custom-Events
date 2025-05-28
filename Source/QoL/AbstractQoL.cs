using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal abstract class AbstractQoL : CustomEventsHandler
	{
		protected Config _config;

		protected AbstractQoL()
		{
			CustomHandlersManager.RegisterEventsHandler(this);
			_config = ArdasCustomEventsPlugin.Instance.Config;
		}

		~AbstractQoL()
		{
			CustomHandlersManager.UnregisterEventsHandler(this);
		}

		protected void Info(object info)
		{
			Logger.Info($"{GetType()}: {info}");
		}
	}
}
