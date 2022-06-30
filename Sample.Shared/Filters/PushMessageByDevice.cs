using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Shared.Filters
{
    public class BasePushParams
    {
        public BasePushParams()
        {

        }
        public string AppCode { get; set; } = String.Empty;
        public string Message { get; set; } = string.Empty;
        public short? Badge { get; set; }
        public string? Sound { get; set; }
        public bool? SendPush { get; set; }
        public string? Param1 { get; set; }
        public string? Param2 { get; set; }
        public string? Param3 { get; set; }
        public string? Param4 { get; set; }
        public string? Param5 { get; set; }
    }

    public class PushMessageByDevice : BasePushParams
    {
        public PushMessageByDevice() :
            base()
        {

        }
        public string[] DeviceIds { get; set; } = new string[0];
        public string FirstDeviceId {
            get
            {
                return DeviceIds[0];
            }
            set
            {
                if (DeviceIds == null || DeviceIds.Length==0)
                    DeviceIds = new string[1];

                DeviceIds[0] = value;
            }
        }
    }


}
