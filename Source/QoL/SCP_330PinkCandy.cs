using InventorySystem;
using InventorySystem.Items.Usables.Scp330;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal class SCP_330PinkCandy : AbstractQoL
	{
		private Random _random = new();

		public override void OnPlayerInteractingScp330(PlayerInteractingScp330EventArgs ev)
		{
			if (!ev.IsAllowed)
				return;

			if (_random.Next(0, 100) < _config.PinkCandyGrabChance)
			{
				ev.CandyType = CandyKindID.Pink;
			}
		}
	}
}
