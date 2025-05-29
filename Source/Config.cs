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
		public int EventChance { get; set; } = 20;

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
	}
}
