using System.Runtime.InteropServices;

namespace AspNetCoreApplication.Services
{
    public class HomeService : IHomeService
    {
        public string About
        {
            get
            {
                return RuntimeInformation.OSDescription;
            }
        }

        public string Contact
        {
            get
            {
                return "Your contact page.";
            }
        }
    }
}
