using UnityEngine;
using System.Collections;
namespace Game.Scripts.AI.Goals
{
	public class CollectResourceGoal : Game.Scripts.AI.Core.ReGoapGoal<string, object>
	{
		public string ResourceName;

		protected override void Awake()
		{
			base.Awake();
			goal.Set("collectedResource" + ResourceName, true);
		}

		public override string ToString()
		{
			return string.Format("GoapGoal('{0}', '{1}')", Name, ResourceName);
		}
	}
}