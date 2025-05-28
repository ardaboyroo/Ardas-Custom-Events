using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal class PlayerJoinedBroadcast : AbstractQoL
	{
		private bool _inLobby = true;

		public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
		{
			Logger.Info($"{ev.Player.DisplayName} joined the game");
			Server.SendBroadcast($"{ev.Player.DisplayName} joined the game", 5, Broadcast.BroadcastFlags.Normal, _inLobby);
		}

		public override void OnPlayerLeft(PlayerLeftEventArgs ev)
		{
			// Player display name is null...
			//Logger.Info($"{ev.Player.DisplayName} left the game");
			//Server.SendBroadcast($"{ev.Player.DisplayName} left the game", 5, Broadcast.BroadcastFlags.Normal, _inLobby);
		}

		public override void OnServerRoundStarted()
		{
			_inLobby = false;
		}

		public override void OnServerRoundEnded(RoundEndedEventArgs ev)
		{
			_inLobby = true;
		}
	}
}
