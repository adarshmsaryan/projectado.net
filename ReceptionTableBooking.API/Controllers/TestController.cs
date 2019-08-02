using System.Web.Http;

namespace ReceptionTableBooking.API.Controllers
{
    public class TestController : ApiController
    {
        public class user
        {
            public string username { get; set; }
            public string password { get; set; }
        }
        public string GetMeValue()
        {
            return "Value";
        }
        public string valid(user u)
        {
            if(u.username=="adarsh" && u.password=="123")
            {
                return "succes";
            }
            else
            {
                return "Error";
            }
        }

    
    }
}
