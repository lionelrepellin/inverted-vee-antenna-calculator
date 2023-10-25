using System;

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
