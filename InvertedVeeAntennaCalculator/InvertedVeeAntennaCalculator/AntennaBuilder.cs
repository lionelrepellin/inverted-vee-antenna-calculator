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
		{
			_maxGroundLength = maxGroundLength;
		}

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

	public class AntennaModel
	{
		public int Band { get; set; }
		public double CenterFrequency { get; set; }
		public double TotalLength { get; set; }
		public double Height { get; set; }
		public double MaxElevation { get; set; }
	}
}
