namespace Sample.Shared.DTO
{
    public class DeviceDTO
    {
        public short DeviceType { get; set; }
        public string Token { get; set; } = null!;
        //public long AppId { get; set; }
        public string DevicePlatformId { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Expired { get; set; }
        public string Language { get; set; } = null!;

        public string appAccessToken { get; set; } = null!;

    }
}
