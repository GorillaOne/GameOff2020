using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorillaTactics.Factories
{
	public interface IDestroyable
	{
		event Action<object> Destroyed; 
		void Destroy(); 
	}
}
