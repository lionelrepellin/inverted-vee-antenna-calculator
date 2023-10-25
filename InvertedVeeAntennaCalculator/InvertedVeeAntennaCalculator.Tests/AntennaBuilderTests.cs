using NFluent;
using NUnit.Framework;
using System;
using System.Linq;

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

            Check.That(models.Where(x => x.MaxElevation == 5).Extracting("Band")).ContainsExactly(17, 15, 12, 10);

            // 0.75 meter max of elevation for 60 meters band
            var sixtyMeters = models.Single(x => x.Band == 60);
            Check.That(sixtyMeters.MaxElevation).IsEqualTo(0.75);

            // 2.25 meters max of elevation for 40 meters band
            var fortyMeters = models.Single(x => x.Band == 40);
            Check.That(fortyMeters.MaxElevation).IsEqualTo(2.25);

            // 3.75 meters max of elevation for 30 meters band
            var thirtyMeters = models.Single(x => x.Band == 30);
            Check.That(thirtyMeters.MaxElevation).IsEqualTo(3.75);
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
            Check.That(Math.Round(model.MinFrequency, 2)).IsEqualTo(4.75); // MHz
            Check.That(Math.Round(model.AntennaLength, 2)).IsEqualTo(29.99); // meter
            Check.That(Math.Round(model.GroundLength, 2)).IsEqualTo(25.97); // meter
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
