using LabApi.Events.CustomHandlers;
using LabApi.Events.Handlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal abstract class AbstractCustomEvent : CustomEventsHandler, IDisposable
	{
		public abstract string Title { get; }
		public abstract string Description { get; }
		public abstract bool ShouldUseBalancedRoles { get; }

		protected Config _config;

		protected AbstractCustomEvent()
		{
			CustomHandlersManager.RegisterEventsHandler(this);
			_config = ArdasCustomEventsPlugin.Instance.Config;
			ServerEvents.RoundStarted += BroadcastEvent;
		}

		public void BroadcastEvent()
		{
			Server.SendBroadcast($"<size=100><color=yellow>{Title}</color></size>\n{Description}", 20, 0, true);
		}

		protected void Info(object info)
		{
			Logger.Info($"{GetType()}: {info}");
		}

		public virtual void Dispose()
		{
			CustomHandlersManager.UnregisterEventsHandler(this);
			ServerEvents.RoundStarted -= BroadcastEvent;
		}
	}
}
