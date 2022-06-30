using NetTopologySuite.Geometries;

using System.Globalization;

namespace Sample.Machine2Machine.DTO
{
    public class PushMessageDTO
    {
        public string PushMessageId { get; set; } = string.Empty;
        public string? Message { get; set; }
        public short? Badge { get; set; }
        public string? Sound { get; set; }
        public bool SendApns { get; set; }
        public bool SendC2dm { get; set; }
        public bool SendMpns { get; set; }
        public bool SendBpns { get; set; }
        public DateTime Created { get; set; }
        public bool? SendPush { get; set; }
        //public Geometry? Geometry { get; set; }

        public string appAccessToken { get; set; } = null!;

        public virtual ICollection<PushParamDTO> PushParams { get; set; } = new List<PushParamDTO>();
        public virtual ICollection<TagFilterDTO> TagFilters { get; set; } = new List<TagFilterDTO>();

    }

}
