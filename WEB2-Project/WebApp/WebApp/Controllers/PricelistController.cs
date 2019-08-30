using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Lists;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;
using static WebApp.Models.Enums;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/PriceList")]
    public class PricelistController : ApiController
    {

        private IUnitOfWork db;
        public PricelistController(IUnitOfWork db)
        {
            this.db = db;
        }

        [Authorize(Roles = "Admin")]
        [Route("GetPriceListAdmin")]
        // GET: api/PriceList/GetPriceListAdmin
        public IEnumerable<PricelistLine> GetPriceListAdmin()
        {
            List<Pricelist> priceLists = db.Pricelists.GetAll().ToList();
            List<PricelistLine> ret = new List<PricelistLine>();
            //privremeno resenje citanja iz baze
            List<Price> prices = db.Prices.GetAll().ToList();

            foreach (var v in priceLists)
            {
                foreach (var price in prices)
                {
                    if (price.IdPrice == v.IdPriceList)
                    {
                        PricelistLine p = new PricelistLine();
                        p.ValidFrom = v.StartDate;
                        if (price.Type == Enums.TicketType.Hourly)
                        {
                            p.TypeOfTicket = "One-hour";
                        }
                        else if (price.Type == Enums.TicketType.Daily)
                        {
                            p.TypeOfTicket = "Day";
                        }
                        else if (price.Type == Enums.TicketType.Monthly)
                        {
                            p.TypeOfTicket = "Mounth";
                        }
                        else if (price.Type == Enums.TicketType.Annual)
                        {
                            p.TypeOfTicket = "Year";
                        }
                        //p.TypeOfTicket = Int32.Parse();//Int32.Parse(db.Prices.GetAll().FirstOrDefault(u => u.Type == price.Type).Type.ToString());
                        p.Value = price.Value;
                        p.IDPrice = price.IdPrice;
                        p.IDPriceList = v.IdPriceList;
                        ret.Add(p);
                    }
                }
            }

            return ret.OrderBy(o => o.ValidFrom).ToList(); ;
        }

        // GET: api/PriceLists/5
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult GetPriceList(int id)
        {
            Pricelist priceList = db.Pricelists.Get(id);
            if (priceList == null)
            {
                return NotFound();
            }

            return Ok(priceList);
        }

        // PUT: api/PriceLists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPriceList(int id, Pricelist priceList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceList.IdPriceList)
            {
                return BadRequest();
            }

            db.Pricelists.Update(priceList);

            try
            {
                db.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostPriceListLine")]
        // POST: api/PriceList/addPriceListLine
        [ResponseType(typeof(Pricelist))]
        public string PostPriceListLine(PricelistLine priceListLine)
        {
            if (!ModelState.IsValid)
            {
                return "bad";
            }
            if (priceListLine == null)
            {
                return "null";
            }

            Pricelist priceListExist = db.Pricelists.GetAll().FirstOrDefault(u => u.StartDate == priceListLine.ValidFrom);
            TicketType id = Enums.TicketType.Hourly;

            if (priceListLine.TypeOfTicket == "One-hour")
                id = Enums.TicketType.Hourly;
            else if (priceListLine.TypeOfTicket == "Day")
                id = Enums.TicketType.Daily;
            else if (priceListLine.TypeOfTicket == "Mounth")
                id = Enums.TicketType.Monthly;
            else if (priceListLine.TypeOfTicket == "Year")
                id = Enums.TicketType.Annual;

            Price priceExist = db.Prices.GetAll().FirstOrDefault(u => (u.Value == priceListLine.Value && u.Type == id));
            Price newPrice = new Price();
            if (priceExist == null)
            {
                newPrice.Pricelists = new List<Pricelist>();
                newPrice.Value = priceListLine.Value;
                newPrice.Type = id;
            }

            if (priceListExist == null)
            {
                Pricelist newPriceList = new Pricelist() { StartDate = priceListLine.ValidFrom, EndDate = priceListLine.ValidFrom, Prices = new List<Price>() };
                newPriceList.Prices = new List<Price>();
                if (priceExist == null)
                {
                    try
                    {
                        newPriceList.Prices.Add(newPrice);
                        db.Pricelists.Add(newPriceList);
                        newPrice.Pricelists.Add(newPriceList);
                        db.Prices.Add(newPrice);
                        db.Complete();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    newPriceList.Prices.Add(priceExist);
                    db.Pricelists.Add(newPriceList);
                    priceExist.Pricelists.Add(newPriceList);
                    db.Prices.Update(priceExist);
                }
            }
            else
            {                
                if (priceListExist.Prices != null)
                {
                    foreach (Price p in priceListExist.Prices)
                    {
                        if (p.Type == id)
                        {
                            return "type of ticket for this price list exists!";
                        }
                    }
                }

                if (priceExist == null)
                {
                    priceListExist.Prices = new List<Price>();
                    priceListExist.Prices.Add(newPrice);
                    db.Pricelists.Update(priceListExist);
                    newPrice.Pricelists.Add(priceListExist);
                    db.Prices.Add(newPrice);
                }
                else
                {
                    priceListExist.Prices.Add(priceExist);
                    db.Pricelists.Update(priceListExist);
                    priceExist.Pricelists.Add(priceListExist);
                    db.Prices.Update(priceExist);
                }
            }

            try
            {
                db.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "ok";
        }
        [Authorize(Roles = "Admin")]
        [Route("EditPriceListLine")]
        // POST: api/PriceList/EditPriceListLine
        [ResponseType(typeof(void))]
        public string EditPriceListLine([FromBody]PricelistLine priceListLine)
        {
            if (!ModelState.IsValid)
            {
                return "bad";
            }
            if (priceListLine == null)
            {
                return "null";
            }

            TicketType type = Enums.TicketType.Hourly;//(TicketType)priceListLine.TypeOfTicket;//db.Prices.GetAll().FirstOrDefault(u => u.Type == (TicketType)priceListLine.TypeOfTicket).Type;

            if (priceListLine.TypeOfTicket == "One-hour")
                type = Enums.TicketType.Hourly;
            else if (priceListLine.TypeOfTicket == "Day")
                type = Enums.TicketType.Daily;
            else if (priceListLine.TypeOfTicket == "Mounth")
                type = Enums.TicketType.Monthly;
            else if (priceListLine.TypeOfTicket == "Year")
                type = Enums.TicketType.Annual;

            Pricelist priceListExist = db.Pricelists.GetAll().FirstOrDefault(u => u.StartDate == priceListLine.ValidFrom);
            TicketType id = db.Prices.GetAll().FirstOrDefault(u => u.Type == type).Type;
            Price priceExist = db.Prices.GetAll().FirstOrDefault(u => (u.IdPrice == priceListExist.IdPriceList));
            Price newPrice = new Price();

            Pricelist priceList = new Pricelist() { StartDate = priceListLine.ValidFrom, EndDate = priceListLine.ValidFrom, Prices = new List<Price>() };

            //if (priceExist == null)
            //{

            newPrice.Pricelists = new List<Pricelist>();
            newPrice.Value = priceListLine.Value;
            newPrice.Type = db.Prices.GetAll().FirstOrDefault(u => u.Type == type).Type;
            //}

            priceList.Prices.Add(newPrice);
            Price priceFromBase = db.Prices.GetAll().FirstOrDefault(u => u.IdPrice == priceListLine.IDPrice);

            //db.PriceLists.Update(exist);
            if (priceExist != null)
                db.Prices.Remove(priceExist);
            //newPrice.Pricelists.Add(exist);
            db.Prices.Add(newPrice);
            if (priceListExist != null)
                db.Pricelists.Remove(priceListExist);
            db.Pricelists.Add(priceList);           

            try
            {
                db.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "ok";
        }

        [Authorize(Roles = "Admin")]
        [Route("DeleteLine/{IDPriceList}/{IDPrice}")]
        // DELETE: api/PriceLists/5
        [ResponseType(typeof(Pricelist))]
        public IHttpActionResult DeletePriceList(int IdPriceList, int IDPrice)
        {
            Pricelist priceList = db.Pricelists.GetAll().FirstOrDefault(u => u.IdPriceList == IdPriceList);
            if (priceList == null)
            {
                return NotFound();
            }

            Price price = db.Prices.GetAll().FirstOrDefault(u => u.IdPrice == IDPrice);
            //priceList.Prices.Remove(price);

            db.Pricelists.Remove(priceList);
            db.Complete();

            return Ok(priceList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceListExists(int id)
        {
            return db.Pricelists.GetAll().Count(e => e.IdPriceList == id) > 0;
        }
    }
}
