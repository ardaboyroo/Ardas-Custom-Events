using CustomPlayerEffects;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal class Config
	{
		[Description("The chance of an event occuring, doubles consecutively when no event is chosen.")]
		public int EventChance { get; set; } = 30;

		[Description("Primary player SCP's, you can also use scp ID.")]
		public RoleTypeId[] PrimaryScp { get; set; } =
		{
			RoleTypeId.Scp173,
			RoleTypeId.Scp939,
			RoleTypeId.Scp096
		};

		[Description("Secondary player SCP's, you can also use scp ID.")]
		public RoleTypeId[] SecondaryScp { get; set; } =
		{
			RoleTypeId.Scp3114,
			RoleTypeId.Scp049,
			RoleTypeId.Scp106,
			RoleTypeId.Scp079
		};

		[Description("Chance of player grabbing pink candy from SCP-330, set to 0 to disable.")]
		public int PinkCandyGrabChance = 5;

		[Description("Item pool for coin flip.")]
		public List<ItemType> CoinItems { get; set; } =
		[
			ItemType.Medkit,
			ItemType.Flashlight,
			ItemType.AntiSCP207,
			ItemType.SCP207,
			ItemType.Adrenaline,
			ItemType.Coin,
			ItemType.SCP1853,
			ItemType.SCP1576,
			ItemType.GrenadeFlash,
			ItemType.GrenadeHE,
			ItemType.SCP018,
			ItemType.SCP244a,
			ItemType.SCP244b,
			ItemType.ArmorLight,
			ItemType.ArmorCombat,
			ItemType.GunCOM15,
			ItemType.GunCOM18,
			ItemType.GunRevolver,
			ItemType.GunFSP9,
			ItemType.GunCrossvec,
			ItemType.KeycardResearchCoordinator,
			ItemType.KeycardZoneManager,
			ItemType.Painkillers
		];

		[Description("Effects pool for coin flip. The number represents the intensity")]
		public List<Tuple<string, int>> CoinEffects { get; set; } =
		[
			// Positive
			new(nameof(BodyshotReduction), 3),
			new(nameof(DamageReduction), 40),
			new(nameof(Invigorated), 1),
			new(nameof(Invisible), 1),
			new(nameof(MovementBoost), 15),
			new(nameof(RainbowTaste), 1),
			new(nameof(Vitality), 1),

			// Negative
			new(nameof(Bleeding), 1),
			new(nameof(Blurred), 1),
			new(nameof(Burned), 1),
			new(nameof(Concussed), 1),
			new(nameof(Corroding), 1),
			new(nameof(Deafened), 1),
			new(nameof(Disabled), 1),
			new(nameof(Exhausted), 1),
			new(nameof(Hemorrhage), 1),
			new(nameof(Poisoned), 1),
			new(nameof(Slowness), 15),
		];

		[Description("BlackoutEvent: The time it takes before a respawn wave happens for Class D")]
		public int BlackoutRespawnWaveTimer { get; set; } = 300;

		[Description("BlackoutEvent: The items Class D spawns with during Blackout.")]
		public ItemType[] BlackoutClassDItems { get; set; } =
		{
			ItemType.Flashlight,
			ItemType.SCP2176,
			ItemType.GrenadeFlash
		};

		[Description("BlackoutEvent: The items the escaped Class D receives when spawned as Chaos.")]
		public ItemType[] BlackoutChaosItems { get; set; } =
		{
			ItemType.KeycardChaosInsurgency,
			ItemType.Medkit,
			ItemType.ArmorCombat,
			ItemType.ParticleDisruptor,
			ItemType.GunLogicer,
			ItemType.Ammo762x39,
		};

		[Description("ItemEvent: Determines if every player should get a random item or the same.")]
		public bool ItemEventRandomitem { get; set; } = true;

		[Description("ItemEvent: A list of items players can get during Item Event, you can also use item ID.")]
		public ItemType[] ItemEventItems { get; set; } =
		{
			ItemType.Adrenaline,
			ItemType.AntiSCP207,
			ItemType.Flashlight,
			ItemType.GrenadeFlash,
			ItemType.GrenadeHE,
			ItemType.Lantern,
			ItemType.Medkit,
			ItemType.Painkillers,
			ItemType.SCP018,
			ItemType.SCP1344,
			ItemType.SCP1853,
			ItemType.SCP207,
			ItemType.SCP2176,
			ItemType.SCP244a,
			ItemType.SCP268,
			ItemType.SCP330,
			ItemType.SCP500
		};

		[Description("ClusterEvent: Amount of live grenades that drop when a player dies.")]
		public int ClusterAmount { get; set; } = 5;

		[Description("Team Deathmatch: Number of rounds played consecutively.")]
		public int TdmRoundLimit { get; set; } = 2;

	}
}
