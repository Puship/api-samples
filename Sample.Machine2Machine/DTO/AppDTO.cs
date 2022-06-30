namespace Sample.Machine2Machine.DTO
{
    public class AppDTO
    {
        public string Name { get; set; } = String.Empty;
        public string AccessCode { get; set; } = null!;
        public bool EnableAPNS { get; set; }
        public bool EnableGCM { get; set; }
        public bool EnableMPNS { get; set; }
        public bool EnableBPNS { get; set; }
        public bool Development { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

    }
}
