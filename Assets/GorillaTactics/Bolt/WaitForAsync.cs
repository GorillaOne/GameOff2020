using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolt;
using Ludiq;
using UnityEngine;

namespace GorillaTactics.BoltUnits
{
	public class WaitForAsync : Bolt.WaitUnit
	{
		[DoNotSerialize]
		public ValueInput taskInput;

		Task task;
		protected override IEnumerator Await(Flow flow)
		{
			task = flow.GetValue<Task>(taskInput);
			while (!task.IsCompleted)
			{
				yield return null;
			}
			yield return exit;
		}

		protected override void Definition()
		{
			base.Definition();
			taskInput = ValueInput<Task>("taskInput", null);
		}
	}
}
