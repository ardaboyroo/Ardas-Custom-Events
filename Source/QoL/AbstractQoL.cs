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
		private bool _enabled = false;
		public bool Enabled
		{
			get
			{
				return _enabled;
			}

			set
			{
				if (_enabled == value)
					return;
				_enabled = value;
				if (value)
					Enable();
				else
					Disable();
			}
		}

		public AbstractQoL()
		{
			Enabled = true;
		}

		~AbstractQoL()
		{
			Enabled = false;
		}

		protected void Info(object info)
		{
			Logger.Info($"{GetType()}: {info}");
		}

		private void Enable()
		{
			CustomHandlersManager.RegisterEventsHandler(this);
			_config = ArdasCustomEventsPlugin.Instance.Config;
		}

		protected void Disable()
		{
			CustomHandlersManager.UnregisterEventsHandler(this);
		}
	}
}
