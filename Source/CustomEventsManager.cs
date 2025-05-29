using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal class CustomEventsManager : CustomEventsHandler
	{
		private Config _config;
		private Random _random = new();
		private AbstractCustomEvent _currentEvent;

		public AbstractCustomEvent CurrentEvent
		{
			get
			{
				if (_debugEvent != null)
					return _debugEvent;
				else
					return _currentEvent;
			}
		}
		private AbstractCustomEvent _debugEvent;
		private int _eventChance;
		private List<Type> _availableEvents;
		private int _eventCounter = 0;

		public CustomEventsManager()
		{
			CustomHandlersManager.RegisterEventsHandler(this);
			_config = ArdasCustomEventsPlugin.Instance.Config;
			RegisterAllCustomEvents();
		}

		~CustomEventsManager()
		{
			CustomHandlersManager.UnregisterEventsHandler(this);
		}

		public override void OnServerRoundStarting(RoundStartingEventArgs ev)
		{
			_currentEvent?.Dispose();
			_currentEvent = null;
			

			if (_debugEvent != null)
			{
				Logger.Info($"Debug event set to: {_debugEvent.GetType().Name}");
				return;
			}

			if (_availableEvents.Any() && _random.Next(0, 100) < _eventChance)
			{
				Logger.Info($"Event chosen with chance: {_eventChance}");
				_eventChance = 0;

				ChooseNextEvent();
				Logger.Info($"Current Event: {_currentEvent.GetType().Name}");

			}
			else
			{
				_eventChance += _eventChance == 0 ? _config.EventChance : _eventChance;
				Logger.Info($"No event chosen, chance increased to: {_eventChance}");
			}
		}

		private void RegisterAllCustomEvents()
		{
			// Reflection magic...
			Type baseType = typeof(AbstractCustomEvent);

			IEnumerable<Type> implementingTypes =
				AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => baseType.IsAssignableFrom(type)
							   && type.IsClass
							   && !type.IsAbstract);

			_availableEvents = implementingTypes.ToList();
			_availableEvents.Shuffle();
		}

		private void ChooseNextEvent()
		{
			//Type type = _availableEvents.ElementAt(_random.Next(0, _availableEvents.Count));

			Type type = _availableEvents.ElementAt(_eventCounter);

			// Create an instance using the default constructor
			if (Activator.CreateInstance(type) is AbstractCustomEvent instance)
			{
				_currentEvent = instance;
			}

			_eventCounter = (_eventCounter + 1) % _availableEvents.Count;
		}
	}
}
