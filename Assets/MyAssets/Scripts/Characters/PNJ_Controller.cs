using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PNJ_Controller : MonoBehaviour {

	/// <summary>
	/// Indicate task name
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class TaskName : System.Attribute
	{

		// Task name
		private string _name;

		/// <summary>
		/// Task name
		/// </summary>
		public string Name { get { return _name; } }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">Task name</param>
		public TaskName(string value)
		{
			this._name = value;
		}
		
	}
	
	/// <summary>
	/// Indicate if a method is a percept
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class PerceptMethod : System.Attribute {}
	
	/// <summary>
	/// Indicate if a method is an action
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class ActionMethod : System.Attribute {}
        
	/// <summary>
	/// Indicate percept links with actions and weights
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class ActionLink : System.Attribute
	{

		// Action name
		private string _name;
            
		// Link weight
		private float _weight;
		
		/// <summary>
		/// Action name
		/// </summary>
		public string Name { get { return _name; } }
            
		/// <summary>
		/// Link weight
		/// </summary>
		public float Weight { get { return _weight; } }
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">Action name</param>
		/// <param name="weight">Link weight</param>
		public ActionLink(string name, float weight)
		{
			this._name = name;
			this._weight = weight;
		}

	}

	// Decision tree
	private DecisionTree<Action> _decision;

	/// <summary>
	/// Decision tree
	/// </summary>
	public DecisionTree<Action> Decision { get { return _decision; } }
	
	// Use this for initialization
	protected virtual void Start()
	{
		_decision = new DecisionTree<Action>();
		System.Reflection.MethodInfo[] methods = this.GetType().GetMethods();
		foreach (System.Reflection.MethodInfo method in methods) {
			if (((ActionMethod[])method.GetCustomAttributes(typeof(ActionMethod), true)).Length > 0)
				_decision.AddAction(new DecisionTree<Action>.Action(method.Name, (Action)Delegate.CreateDelegate(typeof(Action), this, method)));
		}
		foreach (System.Reflection.MethodInfo method in methods) {
			if (((PerceptMethod[])method.GetCustomAttributes(typeof(PerceptMethod), true)).Length > 0) {
				List<DecisionTree<Action>.Action> actions = new List<DecisionTree<Action>.Action>();
				List<float> weights = new List<float>();
				foreach (ActionLink link in (ActionLink[])method.GetCustomAttributes(typeof(ActionLink), true)) {
					DecisionTree<Action>.Action a = _decision.GetAction(link.Name);
					if (a != null) {
						actions.Add(a);
						weights.Add(link.Weight);
					}
				}
				if (actions.Count > 0)
					_decision.AddPercept(new DecisionTree<Action>.Percept(method.Name, actions.ToArray(), weights.ToArray()));
			}
		}
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		_decision.Reset();
        foreach (System.Reflection.MethodInfo method in this.GetType().GetMethods())
        {
            if (((PerceptMethod[])method.GetCustomAttributes(typeof(PerceptMethod), true)).Length > 0)
            {
                DecisionTree<Action>.Percept p = _decision.GetPercept(method.Name);
				if (p != null && ((Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), this, method))())
					p.Activate();
			}
        }
        Action action = _decision.Decide();
        if (action != null)
            action();
	}
	
}
