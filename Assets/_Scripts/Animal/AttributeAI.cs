using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Animal
{
	public class AttributeAI : MonoBehaviour
	{
		public float max; // the max attribute value
		public float start; // the starting attribute value
		public float regenPerSecond; // the amount of attribute to add per second of game time
		public float regenDelaySeconds; // the number of seconds to pause regenerating after a value change
		public Image bar; // UI bar
		public Transform ui;
		protected Animator animator;
		float delaySeconds; // current delay seconds value
		public float _current;
		protected float current
		{
			set
			{
				_current = value;
				if (bar != null)
					bar.fillAmount = current / max;
			}
			get
			{
				return _current;
			}
		}
		float regenTimer;
		float delayTimer;

		public virtual void Start()
		{
			animator = GetComponent<Animator>();
			ui = bar.transform.parent;
			current = start;
		}

		public virtual void Update()
		{
			Vector3 targetPostition = new Vector3(Camera.main.transform.position.x,
										Camera.main.transform.position.y,
										Camera.main.transform.position.z);
			ui.LookAt(targetPostition);
			if (delaySeconds > 0) // if we are in a delay lets see if it's done 
			{
				delayTimer += Time.deltaTime;
				if (delayTimer >= delaySeconds) // we've completed our delay
				{
					delaySeconds = 0;
					regenTimer = 0; // start a new generating timer
				}
			}
			if (delaySeconds == 0) // if we're not in a delay state lets regen!
			{
				regenTimer += Time.deltaTime;
				if (regenTimer > 1) // regens are per second
				{
					current = Mathf.Clamp(current + regenPerSecond, 0, max);
					regenTimer -= 1; // remove a second from the clock
				}
			}
		}

		public void Inc(float amount)
		{
			current = Mathf.Clamp(current + amount, 0, max);
			regenTimer = 0;
			delaySeconds = regenDelaySeconds;
			delayTimer = 0;
		}

		public float getCurrent()
		{
			return current;
		}

		public int getCurrentPercent()
		{
			return Mathf.CeilToInt(max / current);
		}

	}
}