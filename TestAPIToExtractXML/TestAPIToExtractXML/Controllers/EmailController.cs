using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using TestAPIToExtractXML.Services;
using TestAPIToExtractXML.Model;
using TestAPIToExtractXML.Interfaces;
namespace TestAPIToExtractXML.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private EmailExtractService emailExtractService;
        public EmailController(IEmailExtractService _emailExtractService)
        {
            emailExtractService = new EmailExtractService(_emailExtractService);
        }
        [HttpPost]
        public IActionResult Email_GetToExtract(String email)
        {
            ExtraxtedData data = new ExtraxtedData();
            if (emailExtractService.ValidateMessage(email))
            {              
                //fetch extracted data after calculation
                data = emailExtractService.ExtractEmailContent(email);
                if (data.CostCentre == "UNKNOWN")
                {
                    data.StatusMessage = "Missing Cost Centre";
                    return BadRequest(data);
                }
                else if(data.Total != null)
                {
                    data.StatusMessage = "Message Successfully Extracted and Calculated";
                    return Ok(data);
                }
                else
                {
                    data.StatusMessage = "Missing Total";
                    return BadRequest(data);
                }
            }
            else
            {
                data.StatusMessage = "Opening tags that have no corresponding closing tag";
                return BadRequest(data);
            }
        }
    }
}
