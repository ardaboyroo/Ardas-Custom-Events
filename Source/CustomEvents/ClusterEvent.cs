using Footprinting;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace arda
{
	internal class ClusterEvent : AbstractCustomEvent
	{
		public override string Title => "Cluster Event";

		public override string Description => "Everyone who dies drops <color=red>live grenades</color>.";

		public override bool ShouldUseBalancedRoles => true;

		private System.Random _random = new();

		public override void OnPlayerDying(PlayerDyingEventArgs ev)
		{
			for (int i = 0; i < _config.ClusterAmount; i++)
			{
				// spawn live grenade on player with random velocity
				var grenade = TimedGrenadeProjectile.SpawnActive(ev.Player.Position, ItemType.GrenadeHE, ev.Player, 4);
				grenade.Rigidbody.linearVelocity += new Vector3(_random.Next(-5, 6), 5, _random.Next(-5, 6));
			}
		}
	}
}
