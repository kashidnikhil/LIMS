namespace MyTraining1101Demo.Configuration.Dto
{
    public class ThemeHeaderSettingsDto
    {
        public bool DesktopFixedHeader => true;

        public bool MobileFixedHeader => true;

        public string MinimizeDesktopHeaderType { get; set; }
    }
}