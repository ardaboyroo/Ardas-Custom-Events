using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal class RoleBalancer : AbstractQoL
	{
		private readonly string _roleDistribution = "4014314031441404134044434414";

		private List<string> _assignablePlayers = new();
		private List<string> _lastRoundScp = new();
		private List<string> _currentRoundScp = new();

		List<RoleTypeId> _primaryScp = new();
		List<RoleTypeId> _secondaryScp = new();

		public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
		{
			//Info($"{ev.Player.DisplayName} assigned to {ev.Player.Role}");
		}

		public override void OnServerRoundStarting(RoundStartingEventArgs ev)
		{
			if (ArdasCustomEventsPlugin.Instance.CustomEventsManager.CurrentEvent != null)
			{
				bool shouldBalance = ArdasCustomEventsPlugin.Instance.CustomEventsManager.CurrentEvent.ShouldUseBalancedRoles;

				if (!shouldBalance)
				{
					Info("Not balancig the roles");
					return;
				}
			}

			Info("Balancing the roles");

			// Get players
			// Get primary and secondary scp list
			// Make sure to not choose players that were scp last game

			_primaryScp = _config.PrimaryScp.ToList();
			_secondaryScp = _config.SecondaryScp.ToList();

			foreach (Player plr in Player.ReadyList)
			{
				_assignablePlayers.Add(plr.UserId);
				//Info($"{plr.DisplayName}, {plr.UserId}");
			}

			_lastRoundScp = _currentRoundScp.ToList();
			_currentRoundScp.Clear();

			foreach (string plr in _lastRoundScp)
			{
				//Info($"removing {Player.Get(plr).DisplayName} from assignable because it was scp last round");
				_assignablePlayers.Remove(plr);
			}

			int scpCounter = 0;

			for (int i = 0; i < Player.ReadyList.Count(); i++)
			{
				//Info($"doing it for player {i}");
				switch (_roleDistribution[i] - '0')
				{
					case 0:
						{
							//Info("now going to assign scp");
							string plrId = GetRandomPlayer(true);
							RoleTypeId role = GetRandomScp(scpCounter);
							Player.Get(plrId).SetRole(role);
							//Info($"{Player.Get(plrId).DisplayName} is now {role}");
							scpCounter++;
							continue;
						}

					case 1:
						{
							//Info("now going to assign guard");
							string plrId = GetRandomPlayer(false);
							RoleTypeId role = RoleTypeId.FacilityGuard;
							Player.Get(plrId).SetRole(role);
							//Info($"{Player.Get(plrId).DisplayName} is now {role}");
							continue;
						}

					case 3:
						{
							//Info("now going to assign scientist");
							string plrId = GetRandomPlayer(false);
							RoleTypeId role = RoleTypeId.Scientist;
							Player.Get(plrId).SetRole(role);
							//Info($"{Player.Get(plrId).DisplayName} is now {role}");
							continue;
						}

					case 4:
						{
							//Info("now going to assign classD");
							string plrId = GetRandomPlayer(false);
							RoleTypeId role = RoleTypeId.ClassD;
							Player.Get(plrId).SetRole(role);
							//Info($"{Player.Get(plrId).DisplayName} is now {role}");
							continue;
						}
				}
			}
		}

		private RoleTypeId GetRandomScp(int scpCounter)
		{
			switch (scpCounter)
			{
				case 0:
					{
						RoleTypeId role = _primaryScp.GetRandom();
						_primaryScp.Remove(role);
						return role;
					}

				case 1:
					{
						RoleTypeId role = _secondaryScp.GetRandom();
						_secondaryScp.Remove(role);
						return role;
					}

				case 2:
					{
						RoleTypeId role = _secondaryScp.GetRandom();
						_secondaryScp.Remove(role);
						return role;
					}

				default:
					{
						RoleTypeId role = _primaryScp.GetRandom();
						_primaryScp.Remove(role);
						return role;
					}
			}
		}

		private string GetRandomPlayer(bool forScp)
		{
			if (!forScp && _lastRoundScp.Any())
			{
				string plr = _lastRoundScp.GetRandom();
				_lastRoundScp.Remove(plr);
				//Info($"{Player.Get(plr).DisplayName} chosen from last round scp list");
				return plr;
			}
			else
			{
				string plr = _assignablePlayers.GetRandom();
				_assignablePlayers.Remove(plr);
				if (forScp)
				{
					_currentRoundScp.Add(plr);
					//Info($"{Player.Get(plr).DisplayName} has been added to scp list for next round");
				}
				//Info($"{Player.Get(plr).DisplayName} chosen from assignable list");
				return plr;
			}
		}
	}
}
