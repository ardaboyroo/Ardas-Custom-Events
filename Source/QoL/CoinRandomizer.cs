using CustomPlayerEffects;
using Hints;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using System;
using System.Linq;
using System.Reflection;
using static PlayerList;

namespace arda
{
	internal class CoinRandomizer : AbstractQoL
	{
		public bool DestroyOnFlip = true;
		private Random _random = new();

		public override void OnPlayerFlippedCoin(PlayerFlippedCoinEventArgs ev)
		{
			int num = _random.Next(0, 3);

			if (DestroyOnFlip)
				ev.Player.RemoveItem(ev.CoinItem);

			// Give item
			if (num == 0)
			{
				var item = _config.CoinItems.GetRandom();
				ev.Player.AddItem(item);
				ev.Player.SendHint($"You got a {item}", 4);
			}

			// Give status effect
			else if (num == 1)
			{
				Tuple<string, int> effect = _config.CoinEffects.GetRandom();
				string name = $"CustomPlayerEffects.{effect.Item1}, Assembly-CSharp";
				Type type = Type.GetType(name);
				if (type != null)
				{
					var method = typeof(Player)
						.GetMethods(BindingFlags.Instance | BindingFlags.Public)
						.FirstOrDefault(m =>
							m.Name == "EnableEffect" &&
							m.IsGenericMethod &&
							m.GetGenericArguments().Length == 1 &&
							m.GetParameters().Length == 3 &&
							m.GetParameters()[0].ParameterType == typeof(byte) &&
							m.GetParameters()[1].ParameterType == typeof(float) &&
							m.GetParameters()[2].ParameterType == typeof(bool)
						);

					// Make the generic method and invoke with arguments
					MethodInfo generic = method.MakeGenericMethod(type);
					generic.Invoke(ev.Player, [(byte)effect.Item2, 60, false]);
					ev.Player.SendHint($"You got the {effect.Item1} status effect", 4);
				}
			}

			else
			{
				ev.Player.SendHint("Your coin broke and you received nothing");
			}
		}
	}
}
