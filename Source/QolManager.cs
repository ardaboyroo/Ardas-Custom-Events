using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal class QolManager : CustomEventsHandler
	{
		private List<AbstractQoL> _QolInstances = new();

		public QolManager()
		{
			CustomHandlersManager.RegisterEventsHandler(this);

			RegisterAllQolInstances();
		}

		~QolManager()
		{
			CustomHandlersManager.UnregisterEventsHandler(this);

			_QolInstances.Clear();
		}

		private void RegisterAllQolInstances()
		{
			// Reflection magic...
			Type baseType = typeof(AbstractQoL);

			IEnumerable<Type> implementingTypes =
				AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => baseType.IsAssignableFrom(type)
							   && type.IsClass
							   && !type.IsAbstract);

			foreach (Type type in implementingTypes)
			{
				if (Activator.CreateInstance(type) is AbstractQoL instance)
				{
					_QolInstances.Add(instance);
				}
			}
		}
	}
}
