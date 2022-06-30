using Sample.Shared.DTO;

namespace Sample.Machine2Machine.Models
{
    public class PushMessageViewModel
    {
        public PagedResponse<PushMessageDTO[]>? pushMessage { get; set; }

        public int pageIndex { get; set; } = 1;
        public string deviceId { get; set; } = string.Empty;
        public string accessCode { get; set; } = string.Empty;
    }
}
