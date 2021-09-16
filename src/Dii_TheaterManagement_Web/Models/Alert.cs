namespace Dii_TheaterManagement_Web.Models
{
// test comment
    public class Alert
    {
        public long Id { get; set; }
        public AlertType AlertType { get; set; }
        public bool ShowBadge { get; set; }
        public string Heading { get; set; }
        public string FirstPartOfContent { get; set; }
        public string LinkHref { get; set; }
        public string LinkContent { get; set; }
        public string SecondPartOfContent { get; set; }
        public string Remarks { get; set; }
    }
}
