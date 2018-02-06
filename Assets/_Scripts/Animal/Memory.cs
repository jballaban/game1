using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Animal
{

	public abstract class RecollectionManager
	{
		float _strength;
		public float strength
		{
			get { return _strength; }
			set { _strength = Mathf.Max(0, value); }
		}
		public abstract void Process();
	}

	public class FadingRecollectionManager : RecollectionManager
	{
		public float fadePerSecond;
		public override void Process()
		{
			strength -= fadePerSecond * Time.deltaTime;
		}
	}

	public class Recollection
	{
		public RecollectionManager manager;
		public object information;
		public object reference;
	}

	public class Memory
	{
		List<Recollection> toRemember = new List<Recollection>();
		List<Recollection> recollections = new List<Recollection>();
		Dictionary<object, List<Recollection>> recollectionIndex = new Dictionary<object, List<Recollection>>();

		public void Process()
		{
			recollections.ForEach(r => r.manager.Process());
			recollections.Where(r => r.manager.strength == 0).ToList().ForEach(r => Forget(r));
			PersistNewKnowledge();
		}

		public Recollection Know(object reference, object information, RecollectionManager manager)
		{
			Recollection recollection = new Recollection() { manager = manager, information = information, reference = reference };
			toRemember.Add(recollection);
			return recollection;
		}

		public void Forget(object reference)
		{
			if (recollectionIndex[reference] == null) return;
			recollectionIndex[reference].ForEach(r => recollections.Remove(r));
			recollectionIndex.Remove(reference);
		}

		public void Forget(Recollection recollection)
		{
			recollections.Remove(recollection);
			if (recollectionIndex[recollection.reference] == null) return;
			recollectionIndex[recollection.reference].Remove(recollection);
			if (recollectionIndex[recollection.reference].Count == 0)
				recollectionIndex.Remove(recollection.reference);
		}

		void PersistNewKnowledge()
		{
			toRemember.ForEach(r => Remember(r));
			toRemember.Clear();
		}

		void Remember(Recollection recollection)
		{
			if (!recollectionIndex.ContainsKey(recollection.reference))
				recollectionIndex.Add(recollection.reference, new List<Recollection>());
			recollectionIndex[recollection.reference].Add(recollection);
			recollections.Add(recollection);
		}

	}
}