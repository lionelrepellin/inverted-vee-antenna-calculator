using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;

namespace InvertedVeeAntennaCalculator.Tests
{
	[TestFixture]
	public class CalculatorServiceTests
	{		
		[Test]
		public void GetTotalLength()
		{
			var service = new CalculatorService(7.1);

			var result = service.GetTotalLength();

			result = Math.Round(result, 2);
			Check.That(result).IsEqualTo(20);
		}

		[Test]
		public void GetHeight()
		{
			var service = new CalculatorService(7.1);

			var result = service.GetHeight();

			result = Math.Round(result, 2);
			Check.That(result).IsEqualTo(5);
		}

		[Test]
		public void GetHeightWithElevation()
		{
			var service = new CalculatorService(7.1, 2);

			var result = service.GetHeight();

			result = Math.Round(result, 2);
			Check.That(result).IsEqualTo(7);
		}

		[Test]
		public void GetGroundLength()
		{
			var service = new CalculatorService(7.1);

			var result = service.GetGroundLength();

			result = Math.Round(result, 2);
			Check.That(result).IsEqualTo(17.32);
		}

		[Test]
		public void GetGroundLengthWithElevation()
		{
			var service = new CalculatorService(7.1, 2);

			var result = service.GetGroundLength();

			result = Math.Round(result, 2);
			Check.That(result).IsEqualTo(24.25);
		}

		[Test]
		public void GetTheRopeLengthToAddForEachPole()
		{
			var service = new CalculatorService(7.1, 2);

			var result = service.GetRopeLengthToAdd();

			result = Math.Round(result, 2);
			Check.That(result).IsEqualTo(4);
		}
	}
}
