using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Shared.Filters
{

    public class BasePushMessageParams :
    BasePushParams
    {
        public BasePushMessageParams()
        {
        }

        public BasePushMessageParams(DateTime? lastPositionDate, int? lastPositionNumber, double? p1Latitude, double? p1Longitude, double? p2Latitude, double? p2Longitude)
        {

            this.LastPositionDate = lastPositionDate;
            this.LastPositionNumber = (lastPositionNumber <= 0) ? int.MaxValue : lastPositionNumber;

            this.P1Latitude = p1Latitude;
            this.P1Longitude = p1Longitude;
            this.P2Latitude = p2Latitude;
            this.P2Longitude = p2Longitude;
        }

        public double? P1Latitude { get; set; } = 1000;
        public double? P1Longitude { get; set; } = 1000;
        public double? P2Latitude { get; set; } = 1000;
        public double? P2Longitude { get; set; } = 1000;

        public DateTime? LastPositionDate { get; set; } = null;
        public int? LastPositionNumber { get; set; } = int.MaxValue;
    }
    public class PushMessageByPlatform : BasePushMessageParams
    {
        public PushMessageByPlatform() : base()
        {

        }
        public PushMessageByPlatform(DateTime? lastPositionDate, int? lastPositionNumber, double? p1Latitude, double? p1Longitude, double? p2Latitude, double? p2Longitude) :
            base(lastPositionDate, lastPositionNumber, p1Latitude, p1Longitude, p2Latitude, p2Longitude)
        {

        }
        public bool SendApns { get; set; }
        public bool SendC2dm { get; set; }
        public bool SendMpns { get; set; }
        public bool SendBpns { get; set; }
        public string[] Tags { get; set; } = new string[0];
    }
}
