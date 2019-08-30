using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;
using static WebApp.Models.Enums;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Tickets")]
    public class TicketsController : ApiController
    {      
      
        private IUnitOfWork db;
        public TicketsController(IUnitOfWork db)
        {
            this.db = db;
        }

        public TicketsController() { }

        [Authorize(Roles = "AppUser")]
        [Route("Tickets")]
        public IEnumerable<Ticket> GetTickets()
        {

            var email1 = Request.GetOwinContext().Authentication.User.Identity.Name;


            return db.Tickets.GetAll().Where(t => t.UserName == email1 && t.Type == Enums.TicketType.Hourly);

        }

        [Authorize(Roles = "AppUser")]
        [Route("AllTickets")]
        public IEnumerable<Ticket> GetAllTickets()
        {

            var email1 = Request.GetOwinContext().Authentication.User.Identity.Name;


            return db.Tickets.GetAll().Where(t => t.UserName == email1);

        }

        [Authorize(Roles = "Controller")]
        [Route("CheckValidation")]
        [ResponseType(typeof(Ticket))]
        public string GetTicket(double Id)
        {
            if (((Id % 1) != 0))
                return "Unvalid ticket id. Try without point ->.";


            int id = Convert.ToInt32(Id);



            DateTime dateTime = new DateTime();

            string result = "Ticket with this id - not found!";
            Ticket ticket = db.Tickets.Get(id);
            if (ticket == null)
            {
                return result;
            }

            //One-hour
            if (ticket.Type == Enums.TicketType.Hourly)
            {

                if (ticket.From == ticket.To)
                {
                    result = "Not checked in yet. Invalid.";

                }
                else if (ticket.From > ticket.To)
                {
                    dateTime = ticket.From.AddHours(1);

                    if (ticket.From > dateTime)
                    {
                        result = "1 hour has expired. Invalid.";
                    }
                    else
                    {
                        result = "Valid ticket!";
                    }
                }

                //Day
            }
            else if (ticket.Type == Enums.TicketType.Daily)
            {
                dateTime = DateTime.Now;

                if (ticket.To == DateTime.Today)
                {
                    result = "Valid ticket!";
                }
                else
                {
                    result = "Day has expired. Invalid";
                }
                //Mounth
            }
            else if (ticket.Type == Enums.TicketType.Monthly)
            {
                dateTime = DateTime.Now;

                if (ticket.To.Month == dateTime.Month && ticket.To.Year == dateTime.Year)
                {
                    result = "Valid ticket";
                }
                else
                {
                    result = "Month has expired. Invalid";
                }

                //Year
            }
            else if (ticket.Type == Enums.TicketType.Annual)
            {
                dateTime = DateTime.Now;

                if (ticket.To.Year == dateTime.Year)
                {
                    result = "Valid ticket";
                }
                else
                {
                    result = "Year has expired. Invalid";
                }
            }

            return result;
        }

        [AllowAnonymous]
        [Route("Buy")]
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult PostTicket(string TypeOfTicket, string UserName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }            

            TicketType type = Enums.TicketType.Hourly;
            if (TypeOfTicket == "One-hour")
                type = Enums.TicketType.Hourly;
            else if (TypeOfTicket == "Day")
                type = Enums.TicketType.Daily;
            else if (TypeOfTicket == "Mounth")
                type = Enums.TicketType.Monthly;
            else if (TypeOfTicket == "Year")
                type = Enums.TicketType.Annual;

            Ticket ticket = new Ticket();
            ticket.From = DateTime.Now;
            ticket.UserName = UserName;
            ticket.Type = type;
            ticket.To = ticket.From;

            db.Tickets.Add(ticket);
            db.Complete();

            var user = Request.GetOwinContext().Authentication.User.Identity.Name;

            if (user == null)//neregistrovan
            {
                MailMessage mail = new MailMessage("izvini.moram@gmail.com", UserName);
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("izvini.moram@gmail.com", "izvinimoram33");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp@gmail.com";
                mail.Subject = "Public City Transport Serbia";
                mail.Body = $"You successfully bought ticket at {DateTime.Now}. {Environment.NewLine} Your ticket id is: {ticket.IdTicket} {Environment.NewLine}Thank you!";

                try
                {
                    client.Send(mail);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return InternalServerError(e);
                }
            }

            return Ok();
        }

        [Authorize(Roles = "AppUser")]
        [Route("CheckIn")]
        [HttpPut]
        public IHttpActionResult CheckInTicket([FromBody]Ticket t)
        {
            Ticket ticket = db.Tickets.Get(t.IdTicket);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.To = DateTime.Now;
            db.Tickets.Update(ticket);

            db.Complete();

            return Ok(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return db.Tickets.GetAll().Count(e => e.IdTicket == id) > 0;
        }
    }
}
