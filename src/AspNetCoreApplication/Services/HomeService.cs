using System;

namespace AspNetCoreApplication.Services
{
    public class HomeService : IHomeService
    {
        public string About
        {
            get
            {
                return "Your application description page.";
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
