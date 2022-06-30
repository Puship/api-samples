using Sample.Shared.DTO;

namespace Sample.Machine2Machine.Models
{
    public class AppViewModel
    {
        public PagedResponse<AppDTO[]>? apps { get; set; }

        public int pageIndex { get; set; } = 1;
    }
}
