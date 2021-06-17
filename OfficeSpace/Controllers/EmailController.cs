using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OfficeSpace.Controllers
{
    public class EmailController : ApiController
    {
        public EmailController()
        {}        

        [HttpGet]
        [Route("api/email/approvedetails")]
        public void ApproveDetails(int ID,string Name)
        {            
            EmailManager manager = new EmailManager();
            manager.ApprovedEmail(Name, ID);           
        }

        [HttpGet]
        [Route("api/email/disapprovedetails")]
        public void DisapproveDetails(int ID, string Name)
        {
            EmailManager manager = new EmailManager();
            manager.Disapproved(Name, ID);
        }
    }
}
