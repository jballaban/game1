using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Animal
{

	public class Recollection
	{
		public float strength;
		public object information;
		public object reference;
	}

	public class Memory : MonoBehaviour
	{
		const float FORGET_RATE = .1f;
		List<Recollection> toRemember = new List<Recollection>();
		List<Recollection> recollections = new List<Recollection>();

		void Update()
		{
			recollections.ForEach(r => r.strength = Mathf.Max(0, r.strength - FORGET_RATE * Time.deltaTime));
			recollections.Where(r => r.strength == 0).ToList().ForEach(r => Forget(r));
			PersistNewKnowledge();
		}

		public Recollection Know(object reference, object information, float strength)
		{
			Recollection recollection = new Recollection() { strength = strength, information = information, reference = reference };
			toRemember.Add(recollection);
			return recollection;
		}

		public void Forget(object reference)
		{
			recollections.RemoveAll(r => r.reference == reference);
		}

		public void Forget(Recollection recollection)
		{
			recollections.Remove(recollection);
		}

		void PersistNewKnowledge()
		{
			toRemember.ForEach(r => Remember(r));
			toRemember.Clear();
		}

		void Remember(Recollection recollection)
		{
			recollections.Add(recollection);
		}

	}
}