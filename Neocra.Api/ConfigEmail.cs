namespace Neocra.Api
{
    public class ConfigEmail
    {
        public string EmailTo { get; }

        public ConfigEmail(string emailTo)
        {
            this.EmailTo = emailTo;
        }
    }
}