using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvertedVeeAntennaCalculator
{
	public class CalculatorService
	{
		private const int X = 142;
		private const int Angle = 60; // degrees

		private double _frequency;
		private double _elevation;
		private double _ratio;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="frequency">Frequency in MHz</param>
		public CalculatorService(double frequency) 
			=> _frequency = frequency;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="frequency">Frequency in MHz</param>
		/// <param name="elevation">Height in meters</param>
		public CalculatorService(double frequency, double elevation)
			: this(frequency)
		{
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
			var length = GetOnePoleLength();
			var radian = GetRadian();
			var groundLength = (Math.Cos(radian) * length) * 2;

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
			var length = GetOnePoleLength();
			var radian = GetRadian();

			return Math.Sin(radian) * length + elevation;
		}
	}
}
