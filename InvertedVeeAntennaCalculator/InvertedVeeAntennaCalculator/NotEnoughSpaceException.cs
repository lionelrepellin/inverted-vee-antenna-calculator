using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvertedVeeAntennaCalculator
{
	public class NotEnoughSpaceException : Exception
	{
		public NotEnoughSpaceException(double availableGroundLength)
			: base($"Not enough space ({availableGroundLength}m) to mount an inverted vee antenna.")
		{
		}
	}
}
