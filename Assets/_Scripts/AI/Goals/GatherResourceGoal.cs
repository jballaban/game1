using UnityEngine;
using System.Collections;
using Game.Scripts.AI.Core;
using Game.Scripts.AI.Actions;

namespace Game.Scripts.AI.Goals
{
	public class GatherResourceGoal : ReGoapGoal<string, object>
	{
		public string ResourceName;
		public float Amount;

		protected override void Awake()
		{
			base.Awake();
			goal.Set(GatherResourceAction.KEY_HASRESOURCE + ResourceName, true);
			if (Amount > 0)
				goal.Set(GatherResourceAction.KEY_HASRESOURCEAMOUNT + ResourceName, Amount);
		}

		public override string ToString()
		{
			return string.Format("GoapGoal('{0}', '{1}')", Name, ResourceName);
		}
	}
}