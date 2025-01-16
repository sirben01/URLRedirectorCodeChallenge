namespace RedirectorService.Models
{
    public class RedirectModel
    {
        public string RedirectUrl { get; set; } = string.Empty;
        public string TargetUrl { get; set; } = string.Empty;
        public int RedirectType { get; set; }
        public bool UseRelative { get; set; }

    }
}
