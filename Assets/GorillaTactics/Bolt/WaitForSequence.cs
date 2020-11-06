using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolt;
using DG.Tweening;
using Ludiq;
using UnityEngine;
using Sequence = DG.Tweening.Sequence; 

namespace GorillaTactics.BoltUnits
{
	public class WaitForSequence : Bolt.WaitUnit
	{
		[DoNotSerialize]
		public ValueInput sequenceInput;

		Sequence seq; 
		
		protected override IEnumerator Await(Flow flow)
		{
			seq = flow.GetValue<Sequence>(sequenceInput); 
			
			while(seq.IsActive()) yield return null;

			yield return exit; 
		}

		protected override void Definition()
		{
			base.Definition();
			sequenceInput = ValueInput<Sequence>("sequenceInput", null); 
		}
	}
}
