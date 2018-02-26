using UnityEngine;
using System.Collections;
using Game.Scripts.AI.Core;

namespace Game.Scripts.AI.Goals
{
	public class GatherResourceGoal : ReGoapGoal<string, object>
	{
		public string ResourceName;

		protected override void Awake()
		{
			base.Awake();
			goal.Set("hasResource" + ResourceName, true);
		}

		public override string ToString()
		{
			return string.Format("GoapGoal('{0}', '{1}')", Name, ResourceName);
		}
	}
}