using UnityEngine;
using System.Collections;
using Game.Scripts.AI.Core;
using Game.Scripts.AI.Actions;

namespace Game.Scripts.AI.Sensors
{
	public class ResourcesBagSensor : ReGoapSensor<string, object>
	{
		private ResourcesBag resourcesBag;

		void Awake()
		{
			resourcesBag = GetComponent<ResourcesBag>();
		}

		public override void UpdateSensor()
		{
			var state = memory.GetWorldState();
			foreach (var pair in resourcesBag.GetResources())
			{
				state.Set(GatherResourceAction.KEY_HASRESOURCE + pair.Key, pair.Value > 0);
			}
		}
	}
}