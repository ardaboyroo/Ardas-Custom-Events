using LabApi.Events.Arguments.PlayerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal class ItemEvent : AbstractCustomEvent
	{
		public override string Title => "Item Event";

		public override string Description => $"Everyone spawns with <color=green>{GetDescription()}</color>.";

		public override bool ShouldUseBalancedRoles => true;

		private ItemType[] _availableItems;
		private ItemType _chosenItem;
		private Random _random;

		public ItemEvent()
		{
			_random = new Random();
			_availableItems = _config.ItemEventItems;
			_chosenItem = _availableItems[_random.Next(0, _availableItems.Length)];
			Info($"ItemEvent chosen item: {_chosenItem.ToString()}");
		}

		private string GetDescription()
		{
			if (_config.ItemEventRandomitem)
			{
				return _chosenItem.ToString();
			}
			else
			{
				return "a random item";
			}
		}

		public override void OnPlayerReceivedLoadout(PlayerReceivedLoadoutEventArgs ev)
		{
			if (_config.ItemEventRandomitem)
			{
				ev.Player.AddItem(_availableItems[_random.Next(0, _availableItems.Length)]);
			}
			else
			{
				ev.Player.AddItem(_chosenItem);
			}
			Info($"{ev.Player.DisplayName} spawned");
		}
	}
}
