using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neocra.Api.dto;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Neocra.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly SendGridClient sendGridClient;
        private readonly ConfigEmail configEmail;

        /// <summary>
        /// Contact API
        /// </summary>
        public ContactController(SendGridClient sendGridClient, ConfigEmail configEmail)
        {
            this.sendGridClient = sendGridClient;
            this.configEmail = configEmail;
        }

        /// <summary>
        /// Post a contact message.
        /// </summary>
        /// <param name="messageContact">Information about the message.</param>
        /// <returns>Information about the success of the send.</returns>
        [HttpPost]
        public async Task<ResultContactDto> Post([FromBody]MessageContactDto messageContact)
        {
            var from = new EmailAddress(messageContact.Email, messageContact.FullName);
            var subject = messageContact.Subject;
            var to = new EmailAddress(this.configEmail.EmailTo, this.configEmail.EmailTo);
            var plainTextContent = messageContact.Content;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent,plainTextContent);
            var response = await this.sendGridClient.SendEmailAsync(msg)
                .ConfigureAwait(false);
            return new ResultContactDto
            {
                Success = response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK
            };
        }
    }
}