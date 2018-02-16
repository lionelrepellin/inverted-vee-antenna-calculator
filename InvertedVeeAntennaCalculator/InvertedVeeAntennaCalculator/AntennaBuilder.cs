using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvertedVeeAntennaCalculator
{
	public class AntennaBuilder
	{
		private int _maxGroundLength;
		private int _maxElevation;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="maxGroundLength">Maximum ground length available (in meters)</param>
		public AntennaBuilder(int maxGroundLength)
			=> _maxGroundLength = maxGroundLength;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="maxGroundLength">Maximum ground length available (in meters)</param>
		/// <param name="maxElevation">Maximum elevation to the ground (in meters)</param>
		public AntennaBuilder(int maxGroundLength, int maxElevation)
			: this(maxGroundLength)
		{
			_maxElevation = maxElevation;
		}

		public IEnumerable<AntennaModel> GetWorkableBands()
		{
			var list = new List<AntennaModel>();
			var bands = GetHamRadioBands().OrderByDescending(x => x.Band);
			CalculatorService service;

			foreach (var model in bands)
			{
				var centerFrequency = model.CenterFrequency;
				var elevationToTest = _maxElevation;
				var bandAdded = false;

				do
				{
					service = new CalculatorService(centerFrequency, elevationToTest);
					var groundLength = service.GetGroundLength();

					if (groundLength <= _maxGroundLength)
					{
						list.Add(new AntennaModel
						{
							Band = model.Band,
							CenterFrequency = centerFrequency,
							MinFrequency = model.MinFrequency,
							Height = service.GetHeight(),
							TotalLength = groundLength,
							MaxElevation = elevationToTest
						});

						bandAdded = true;
					}

					// decrease elevation
					elevationToTest--;
				}
				while (elevationToTest > 0 && !bandAdded);
			}

			return list;
		}

		public AntennaMaxModel GetMaxAntennaLength()
		{
			var workableBands = GetWorkableBands();
			var minWorkableFrequency = workableBands.First().MinFrequency;

			double antennaGroundLength = 0;
			double antennaHeight = 0;
			double antennaLength = 0;
			double minFrequency = 0;

			for (var frequency = minWorkableFrequency; frequency > 0; frequency -= 0.1)
			{
				var service = new CalculatorService(frequency);
				var groundLength = service.GetGroundLength();

				if (groundLength <= _maxGroundLength)
				{
					antennaGroundLength = groundLength;
					antennaLength = service.GetTotalLength();
					antennaHeight = service.GetHeight();					
					minFrequency = frequency;
				}
				else break;
			}

			return new AntennaMaxModel
			{
				Height = antennaHeight,
				MinFrequency = minFrequency,
				AntennaLength = antennaLength,
				GroundLength = antennaGroundLength
			};
		}

		private IEnumerable<BandModel> GetHamRadioBands()
		{
			return new List<BandModel>
			{
				new BandModel(2200, 0.1357, 0.1378),
				new BandModel(630, 0.472, 0.479),
				new BandModel(160, 1.810, 1.850),
				new BandModel(80, 3.500, 3.800),
				new BandModel(40, 7.000, 7.200),
				new BandModel(30, 10.100, 10.150),
				new BandModel(20, 14.000, 14.350),
				new BandModel(17, 18.068, 18.168),
				new BandModel(15, 21.000, 21.450),
				new BandModel(12, 24.890, 24.990),
				new BandModel(10, 28.000, 29.700)
			};
		}

		#region Band model


		private class BandModel
		{
			private int _band;
			private double _minFrequency;
			private double _maxFrequency;
			private double _bandWidth => _maxFrequency - _minFrequency;

			public int Band => _band;
			public double MinFrequency => _minFrequency;
			public double CenterFrequency => _minFrequency + (_bandWidth / 2);

			public BandModel(int band, double minFrequency, double maxFrequency)
			{
				_band = band;
				_minFrequency = minFrequency;
				_maxFrequency = maxFrequency;
			}
		}

		#endregion
	}

	public class AntennaMaxModel
	{
		public double MinFrequency { get; set; }
		public double AntennaLength { get; set; }
		public double GroundLength { get; set; }
		public double Height { get; set; }
	}

	public class AntennaModel
	{
		public int Band { get; set; }
		public double CenterFrequency { get; set; }
		public double MinFrequency { get; set; }
		public double Height { get; set; }
		public double MaxElevation { get; set; }		
		public double TotalLength { get; set; }		
	}

}
