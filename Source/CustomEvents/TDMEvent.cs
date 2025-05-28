using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Pickups;
using InventorySystem.Items;
using InventorySystem;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomPlayerEffects;
using Interactables.Interobjects;
using InventorySystem.Items.Jailbird;

namespace arda
{
	internal class TDMEvent : AbstractCustomEvent
	{
		public override string Title => "Team Deathmatch";

		public override string Description => "Kill everyone on the opposing team.";

		public override bool ShouldUseBalancedRoles => false;

		private List<Player> _firstHalf = new();
		private List<Player> _secondHalf = new();

		public TDMEvent()
		{
			InventoryExtensions.OnItemRemoved += OnItemRemoved;
			InventorySystem.Items.Jailbird.JailbirdItem.OnItemRemoved += tets;
		}

		~TDMEvent()
		{
			InventoryExtensions.OnItemRemoved -= OnItemRemoved;
		}
		
		void tets(ItemBase e)
		{
			Info($"item removed fam: {e.ItemTypeId}");
		}

		private void OnItemRemoved(ReferenceHub rh, ItemBase ib, ItemPickupBase ip)
		{
			Info($"removed item {ib.ItemTypeId}, {ib.ItemTypeId == ItemType.Jailbird}");
			if (ib.ItemTypeId == ItemType.Jailbird)
			{
				Info("que!");
				Pickup bird = Pickup.Get(ip);
				Info($"jailbird removed, was it destroyed: {bird.IsDestroyed}");
				//bird.Destroy();
				Item item = Player.Get(rh).AddItem(ItemType.Jailbird);
				Player.Get(rh).CurrentItem = item;
			}
			Info("just to be sure");
		}

		public override void OnPlayerReceivedLoadout(PlayerReceivedLoadoutEventArgs ev)
		{
			Info($"{ev.Player.DisplayName} is {ev.Player.Role}");
			if (ev.Player.Role == RoleTypeId.ChaosRepressor || ev.Player.Role == RoleTypeId.NtfCaptain)
			{
				ev.Player.ClearInventory();

				ev.Player.AddItem(ItemType.Jailbird);
				ev.Player.AddItem(ItemType.KeycardO5);
				ev.Player.AddItem(ItemType.GrenadeHE);
				ev.Player.AddItem(ItemType.Medkit);
				ev.Player.AddItem(ItemType.ArmorHeavy);
			}
		}

		public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
		{
			if (ev.NewRole.RoleTypeId == RoleTypeId.NtfCaptain || ev.NewRole.RoleTypeId == RoleTypeId.ChaosRepressor)
				ev.Player.EnableEffect<AntiScp207>();
		}

		public override void OnPlayerDying(PlayerDyingEventArgs ev)
		{
			if (ev.Player.Team == Team.ChaosInsurgency)
			{
				if (_firstHalf.Remove(ev.Player))
					Info($"{ev.Player.DisplayName} removed from scp, scp count: {_firstHalf.Count}");
				else
					Info($"could not remove {ev.Player.DisplayName} from scp, scp count: {_firstHalf.Count}");

				if (!_firstHalf.Any())
				{
					Info("going to end the round cuz survivors won");
					Round.End(true);
				}
			}
			else if (ev.Player.Team == Team.FoundationForces)
			{
				if (_secondHalf.Remove(ev.Player))
					Info($"{ev.Player.DisplayName} removed from survivor, survivor count: {_secondHalf.Count}");
				else
					Info($"could not remove {ev.Player.DisplayName} from survivor, survivor count: {_secondHalf.Count}");

				if (!_secondHalf.Any())
				{
					Info("going to end the round cuz scp won");
					Round.End(true);
				}
			}
		}

		public override void OnServerRoundEnding(RoundEndingEventArgs ev)
		{
			Info("received round ending");
			if (_firstHalf.Any())
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.ChaosInsurgency;
				Info("scp won");
			}
			else if (_secondHalf.Any())
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.FacilityForces;
				Info("chaos won");
			}
			else
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.Draw;
				Info("no one won?");
			}
		}

		public override void OnServerRoundStarted()
		{
			Round.IsLocked = true;

			Server.CategoryLimits[ItemCategory.SpecialWeapon] = 2;

			Decontamination.Offset = -999999;

			Warhead.IsLocked = true;

			List<Player> plrList = Player.ReadyList.ToList();
			plrList.Shuffle();
			Info($"list: {Player.List.Count()} playerlist: {Player.ReadyList.Count()}, plrList: {plrList.Count()}");
			foreach (Player plr in plrList)
			{
				Info($"{plr.DisplayName} is on the list to get drafted");
			}

			int half = plrList.Count / 2;

			_firstHalf = plrList.GetRange(0, half);
			_secondHalf = plrList.GetRange(half, plrList.Count - half);

			if (_firstHalf.Any())
			{
				_firstHalf[0].SetRole(RoleTypeId.ChaosRepressor);

				for (int i = 1; i < _firstHalf.Count; i++)
				{
					_firstHalf[i].SetRole(RoleTypeId.ChaosRifleman);
				}
			}

			if (_secondHalf.Any())
			{
				_secondHalf[0].SetRole(RoleTypeId.NtfCaptain);

				for (int i = 1; i < _secondHalf.Count; i++)
				{
					_secondHalf[i].SetRole(RoleTypeId.NtfSergeant);
				}
			}

			// Disable surface door
			foreach (var item in Door.List)
			{
				if (item.DoorName == DoorName.SurfaceGate || item.DoorName == DoorName.HczCheckpoint)
				{
					Info($"door was a {item.DoorName}");
					item.IsOpened = false;
					item.Lock(DoorLockReason.AdminCommand, true);
				}
			}
		}
	}
}
