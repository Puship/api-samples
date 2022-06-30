using Sample.Shared.DTO;

namespace Sample.Machine2Machine.Models
{
    public class DeviceViewModel
    {
        public PagedResponse<DeviceDTO[]>? devices { get; set; }

        public int pageIndex { get; set; } = 1;
        public string accessCode { get; set; } = string.Empty;
    }
}
