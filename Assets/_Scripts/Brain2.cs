using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public enum Goals2
{
	FillHunger,
	//FillStamina
}

public enum GoalWeightings : int
{
	Critical = 5,
	High = 3,
	Normal = 2,
	Low = 1
}

public class Brain2 : MonoBehaviour, IBrain
{
	public int Hunger = 100;
	public int Stamina = 100;
	public void Init()
	{
		foreach (var goal in Enum.GetNames(typeof(Goals2)))
			_goalsWeight.Add(goal, 0);
	}

	private float _costTime = 0;
	public void Tick(IGoap goap)
	{
		_costTime += Time.deltaTime;
		if (_costTime >= 1)
		{
			Hunger -= 2;
			Stamina -= 2;
			_costTime = 0;
		}
	}

	public void Release()
	{ }

	protected Dictionary<int, GoalWeightings> DefaultGoalWeighting = new Dictionary<int, GoalWeightings>() {
		{ 10, GoalWeightings.Critical},
		{ 30, GoalWeightings.High},
		{ 60, GoalWeightings.Normal},
		{ 100, GoalWeightings.Low}
	};

	protected virtual int GetWeight(Goals2 goal)
	{
		int current = 0;
		switch (goal)
		{
			case Goals2.FillHunger: current = Hunger; break;
			//case Goals2.FillStamina: current = Stamina; break;
			default: throw new Exception("Unknown goal " + goal);
		}
		return (int)DefaultGoalWeighting.First(w => w.Key >= current).Value;
	}

	readonly Dictionary<string, int> _goalsWeight = new Dictionary<string, int>();
	readonly Dictionary<string, bool> _sortedTags = new Dictionary<string, bool>();
	public Dictionary<string, bool> NextGoal()
	{
		foreach (var goal in Enum.GetNames(typeof(Goals2)))
		{
			_goalsWeight[goal] = GetWeight((Goals2)Enum.Parse(typeof(Goals2), goal));
		}

		var items = from pair in _goalsWeight
					orderby pair.Value descending
					select pair;

		_sortedTags.Clear();
		foreach (KeyValuePair<string, int> pair in items)
		{
			_sortedTags.Add(pair.Key, true);
		}
		return _sortedTags;
	}

}
