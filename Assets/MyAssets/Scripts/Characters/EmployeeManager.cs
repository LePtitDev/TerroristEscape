using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EmployeeManager : PNJ_Controller {

	///////////////
	/// ACTIONS ///
	///////////////
	
	/// <summary>
	/// No action
	/// </summary>
	[ActionMethod]
	public void Idle() {}
	
	////////////////
	/// PERCEPTS ///
	////////////////

	/// <summary>
	/// Indicate if has found a target
	/// </summary>
	[PerceptMethod]
	public bool HasTarget()
	{
		return false;
	}
	
}
