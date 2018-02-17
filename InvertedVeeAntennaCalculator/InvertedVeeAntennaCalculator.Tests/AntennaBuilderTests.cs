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
	public class AntennaBuilderTests
	{
		private const int MaxGroundLenghtAvailable = 26; // meters

		[Test]
		public void GetAntennasAvailableForMe()
		{
			var builder = new AntennaBuilder(MaxGroundLenghtAvailable);
			var models = builder.GetWorkableBands();

			Check.That(models.Extracting("Band")).ContainsExactly(60, 40, 30, 20, 17, 15, 12, 10);
		}

		[Test]
		public void GetAntennasWithElevationAvailableForMe()
		{
			var builder = new AntennaBuilder(MaxGroundLenghtAvailable, 5);
			var models = builder.GetWorkableBands();

			Check.That(models.Where(x => x.MaxElevation == 5).Extracting("Band")).ContainsExactly(20, 17, 15, 12, 10);

			// 2 meters max of elevation for 40 meters band
			var fortyMeters = models.Single(x => x.Band == 40);
			Check.That(fortyMeters.MaxElevation).IsEqualTo(2);

			// 3 meters max of elevation for 30 meters band
			var thirtyMeters = models.Single(x => x.Band == 30);
			Check.That(thirtyMeters.MaxElevation).IsEqualTo(3);
		}

		[Test]
		public void ThrowsAnExceptionIfNotEnoughSpace()
		{
			var builder = new AntennaBuilder(3);

			Check.ThatCode(() =>
			{
				builder.GetWorkableBands();
			}).Throws<NotEnoughSpaceException>();
		}

		[Test]
		public void GetMaxAntennaLength()
		{
			var builder = new AntennaBuilder(MaxGroundLenghtAvailable);
			var model = builder.GetMaxAntennaLength();
			
			Check.That(Math.Round(model.Height, 2)).IsEqualTo(7.50); // meter
			Check.That(Math.Round(model.MinFrequency, 2)).IsEqualTo(4.73); // MHz
			Check.That(Math.Round(model.AntennaLength, 2)).IsEqualTo(30.01); // meter
			Check.That(Math.Round(model.GroundLength, 2)).IsEqualTo(25.99); // meter
		}
		
		[Test]
		public void LowerThenMinGroundLenghtAllowedThrowsAnException()
		{
			Check.ThatCode(() =>
			{
				new AntennaBuilder(0);
			}).Throws<ArgumentOutOfRangeException>();
		}

		[Test]
		public void ElevationUnderZeroMeterThrowsAnException()
		{
			Check.ThatCode(() =>
			{
				new AntennaBuilder(1, -1);
			}).Throws<ArgumentOutOfRangeException>();
		}
	}
}
