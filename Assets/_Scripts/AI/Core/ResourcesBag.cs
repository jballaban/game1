using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Game.Scripts.AI.Core
{
	public class ResourcesBag : MonoBehaviour
	{
		public Dictionary<string, float> resources;
		public float _trees; // readonly view of tree resources

		void Awake()
		{
			resources = new Dictionary<string, float>();
		}

		public void AddResource(string resourceName, float value)
		{
			if (!resources.ContainsKey(resourceName))
				resources[resourceName] = 0;
			resources[resourceName] += value;
			if (resourceName == "Tree")
				_trees = resources[resourceName];
		}

		public float GetResource(string resourceName)
		{
			var value = 0f;
			resources.TryGetValue(resourceName, out value);
			return value;
		}

		public Dictionary<string, float> GetResources()
		{
			return resources;
		}

		public void RemoveResource(string resourceName, float value)
		{
			resources[resourceName] -= value;
			if (resourceName == "Tree")
				_trees = resources[resourceName];
		}
	}
}