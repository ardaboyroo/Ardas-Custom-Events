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
		}

		public override void Dispose()
		{
			base.Dispose();
			InventoryExtensions.OnItemRemoved -= OnItemRemoved;
		}

		private void OnItemRemoved(ReferenceHub rh, ItemBase ib, ItemPickupBase ip)
		{
			if (ib.ItemTypeId == ItemType.Jailbird)
			{
				Player plr = Player.Get(rh);
				if (plr == null) return;
				if (plr.Role == RoleTypeId.ChaosRepressor || plr.Role == RoleTypeId.NtfCaptain)
				{
					// Jailbird has ran out of charges
					if (ip == null)
					{
						if (plr.Items.Any(item => item.Type == ItemType.GunCOM18)) return;

						Item item = plr.AddItem(ItemType.GunCOM18);
						plr.CurrentItem = item;
						plr.AddAmmo(ItemType.Ammo9x19, 60);
					}
				}
				//	if (ip != null)
				//	{
				//		// dropped on the floor
				//		Pickup bird = Pickup.Get(ip);
				//		bird.Destroy();
				//		Item item = Player.Get(rh).AddItem(ItemType.Jailbird);
				//		Player.Get(rh).CurrentItem = item;
				//	}
				//	else
				//	{
				//		// ran out of charges
				//		Item item = Player.Get(rh).AddItem(ItemType.Jailbird);
				//		Player.Get(rh).CurrentItem = item;
				//	}
			}
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

			if (ev.Player.Role == RoleTypeId.ChaosRifleman)
			{
				ev.Player.RemoveItem(ItemType.Painkillers);
				ev.Player.AddItem(ItemType.GrenadeHE);
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
					Info($"{ev.Player.DisplayName} removed from chaos, chaos count: {_firstHalf.Count}");
				else
					Info($"could not remove {ev.Player.DisplayName} from chaos, chaos count: {_firstHalf.Count}");

				if (!_firstHalf.Any())
				{
					Round.End(true);
				}
			}
			else if (ev.Player.Team == Team.FoundationForces)
			{
				if (_secondHalf.Remove(ev.Player))
					Info($"{ev.Player.DisplayName} removed from mtf, mtf count: {_secondHalf.Count}");
				else
					Info($"could not remove {ev.Player.DisplayName} from mtf, mtf count: {_secondHalf.Count}");

				if (!_secondHalf.Any())
				{
					Round.End(true);
				}
			}
		}

		public override void OnServerRoundEnding(RoundEndingEventArgs ev)
		{
			if (_firstHalf.Any())
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.ChaosInsurgency;
				Info("chaos won");
			}
			else if (_secondHalf.Any())
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.FacilityForces;
				Info("MTF won");
			}
			else
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.Draw;
				Info("no one won");
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
					item.IsOpened = false;
					item.Lock(DoorLockReason.AdminCommand, true);
				}
			}
		}
	}
}
