using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvertedVeeAntennaCalculator
{
	public class CalculatorService
	{
		private const double MinFrequencyAllowed = 0.1; // MHz
		private const double MaxFrequencyAllowed = 30; // MHz
		private const double MinElevationAllowed = 0; // meter
		private const int X = 142;
		private const int Angle = 120; // degrees at the top of the mast

		private double _frequency;
		private double _elevation;
		private double _ratio;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="frequency">Frequency in MHz</param>
		public CalculatorService(double frequency)
		{
			if (frequency < MinFrequencyAllowed || frequency > MaxFrequencyAllowed)
				throw new ArgumentOutOfRangeException(nameof(frequency), $"Frequency should be between {MinFrequencyAllowed} and {MaxFrequencyAllowed} MHz.");

			_frequency = frequency;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="frequency">Frequency in MHz</param>
		/// <param name="elevation">Height in meters</param>
		public CalculatorService(double frequency, double elevation)
			: this(frequency)
		{
			if (elevation < MinElevationAllowed)
				throw new ArgumentOutOfRangeException(nameof(elevation), $"Elevation should not be less than {MinElevationAllowed} meter.");

			_elevation = elevation;
			_ratio = GetAntennaHeight(elevation) / GetAntennaHeight(0);
		}

		/// <summary>
		/// Get total length of the antenna (in meters)
		/// </summary>
		/// <returns></returns>
		public double GetTotalLength()
			=> X / _frequency;

		/// <summary>
		/// Get height of the antenna (in meters)
		/// </summary>
		/// <returns></returns>
		public double GetHeight()
			=> GetAntennaHeight(_elevation);

		/// <summary>
		/// Get the total length of the ground (in meters)
		/// </summary>
		/// <returns></returns>
		public double GetGroundLength()
		{
			var length = GetOnePoleLength(); // hypotenuse
			var radian = GetRadian();
			var groundLength = (Math.Sin(radian) * length) * 2;

			if (_ratio > 0)
			{
				groundLength *= _ratio;
			}

			return groundLength;
		}

		/// <summary>
		/// Get the rope length to add for each pole
		/// </summary>
		/// <remarks>This is the rope length to attach one end of the antenna to the ground</remarks>
		/// <returns></returns>
		public double GetRopeLengthToAdd()
			=> _ratio * GetOnePoleLength() - GetOnePoleLength();

		private double GetOnePoleLength()
			=> GetTotalLength() / 2;

		private double GetRadian()
			=> (Angle / 2) * (Math.PI / 180);

		private double GetAntennaHeight(double elevation)
		{
			var length = GetOnePoleLength(); // hypothenuse
			var radian = GetRadian();

			return Math.Cos(radian) * length + elevation;
		}
	}
}
