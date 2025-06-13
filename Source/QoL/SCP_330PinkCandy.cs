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

			if (_config.PinkCandyGrabChance >= _random.Next(1, 101))
			{
				ev.CandyType = CandyKindID.Pink;
			}
		}
	}
}
