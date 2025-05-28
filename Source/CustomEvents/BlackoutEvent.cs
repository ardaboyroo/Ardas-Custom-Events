using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Wrappers;
using Mirror;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Diagnostics;

namespace arda
{
	internal class BlackoutEvent : AbstractCustomEvent
	{
		public override string Title => "Blackout Event";

		public override string Description => "Every light is out, and... who let the <color=red>dogs</color> out?";

		public override bool ShouldUseBalancedRoles => false;

		private List<Player> _survivors = new();
		private List<Player> _scp = new();

		public override void OnPlayerReceivedLoadout(PlayerReceivedLoadoutEventArgs ev)
		{
			if (ev.Player.Role == RoleTypeId.ClassD)
			{
				foreach (ItemType item in _config.BlackoutClassDItems)
				{
					ev.Player.AddItem(item);
				}
			}
			else if (ev.Player.Role == RoleTypeId.ChaosConscript)
			{
				ev.Player.ClearInventory();
				foreach(ItemType item in _config.BlackoutChaosItems)
				{
					ev.Player.AddItem(item);
				}

				// Prolly better to see what kind of ammo the weapon uses
				ev.Player.SetAmmo(ItemType.Ammo762x39, 180);
				//ev.Player.MaxHumeShield = 50;
				//ev.Player.HumeShield = 50;
				//ev.Player.HumeShieldRegenRate = 0;
			}
		}

		public override void OnServerWaveRespawning(WaveRespawningEventArgs ev)
		{
			if (ev.Wave is MtfWave || ev.Wave is MiniMtfWave || ev.Wave is ChaosWave || ev.Wave is MiniChaosWave)
			{
				Info("a wave is spawning, disabling it");
				ev.IsAllowed = false;
			}
		}

		public override void OnServerRoundEnding(RoundEndingEventArgs ev)
		{
			Info("received round ending");
			if (_scp.Any() )
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
				Info("scp won");
			}
			else if (_survivors.Any())
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.ChaosInsurgency;
				Info("chaos won");
			}
			else
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.Draw;
				Info("no one won?");
			}
		}

		public override void OnPlayerDying(PlayerDyingEventArgs ev)
		{
			Info($"{ev.Player.DisplayName} died as {ev.Player.Team}");
			if (ev.Player.Team == Team.SCPs)
			{
				if (_scp.Remove(ev.Player))
					Info($"{ev.Player.DisplayName} removed from scp, scp count: {_scp.Count}");
				else
					Info($"could not remove {ev.Player.DisplayName} from scp, scp count: {_scp.Count}");

				if (!_scp.Any())
				{
					Info("going to end the round cuz survivors won");
					Round.End(true);
				}
			}
			else if (ev.Player.Team == Team.ClassD || ev.Player.Team == Team.ChaosInsurgency)
			{
				if (_survivors.Remove(ev.Player))
					Info($"{ev.Player.DisplayName} removed from survivor, survivor count: {_survivors.Count}");
				else
					Info($"could not remove {ev.Player.DisplayName} from survivor, survivor count: {_survivors.Count}");

				if (!_survivors.Any())
				{
					Info("going to end the round cuz scp won");
					Round.End(true);
				}
			}
		}

		public override void OnServerRoundStarted()
		{
			// total 20 minutes to decontaminate
			Decontamination.Offset = -300;

			_scp.Clear();
			_survivors.Clear();

			List<Player> plrList = Player.ReadyList.ToList();
			plrList.Shuffle();

			if (plrList.Count() < 8)
			{
				plrList[0].SetRole(RoleTypeId.Scp939);
				_scp.Add(plrList[0]);
				plrList.RemoveAt(0);
			}
			else if (plrList.Count() < 16)
			{
				plrList[0].SetRole(RoleTypeId.Scp939);
				plrList[1].SetRole(RoleTypeId.Scp939);
				_scp.Add(plrList[0]);
				_scp.Add(plrList[1]);
				plrList.RemoveAt(1);
				plrList.RemoveAt(0);
			}
			else
			{
				plrList[0].SetRole(RoleTypeId.Scp939);
				plrList[1].SetRole(RoleTypeId.Scp939);
				plrList[2].SetRole(RoleTypeId.Scp939);
				_scp.Add(plrList[0]);
				_scp.Add(plrList[1]);
				_scp.Add(plrList[2]);
				plrList.RemoveAt(2);
				plrList.RemoveAt(1);
				plrList.RemoveAt(0);
			}

			foreach (Player player in plrList)
			{
				player.SetRole(RoleTypeId.ClassD);
				_survivors.Add(player);
			}

			foreach (var light in Map.RoomLights)
			{
				light.LightsEnabled = false;
			}

			Round.IsLocked = true;

			Server.CategoryLimits[ItemCategory.SpecialWeapon] = 2;
		}
	}
}
