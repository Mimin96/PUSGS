using System;
using System.Collections.Generic;
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
    [RoutePrefix("api/Line")]
    public class LinesController : ApiController
    {
        private IUnitOfWork db;

        public LinesController()
        {

        }

        public LinesController(IUnitOfWork db)
        {

            //var Line = db.Lines.GetAll().Where(l => l.Number == "11").FirstOrDefault();

            //var s1 = db.Stations.GetAll().Where(s => s.Name == "B").FirstOrDefault();
            //var s2 = db.Stations.GetAll().Where(s => s.Name == "mapica").FirstOrDefault();
            //var s3 = db.Stations.GetAll().Where(s => s.Name == "maza").FirstOrDefault();
            //s1.Lines = new List<Line>();
            //s1.Lines.Add(Line);
            //db.Stations.Update(s1);

            //db.Complete();

            this.db = db;
        }

        //////////////////////////////////////////////////////////////4
        [AllowAnonymous]
        [Route("GetLines")]
        public IEnumerable<LineStation> GetLines()
        {
            List<Line> lines = db.Lines.GetAll().ToList();
            List<LineStation> ret = new List<LineStation>();

            foreach (Line l in lines)
            {
                RouteType type = l.RouteType; //db.TypesOfLine.GetAll().FirstOrDefault(u => u.IDtypeOfLine == l.IDtypeOfLine).typeOfLine;
                LineStation lp = new LineStation() { Number = l.Number, IDtypeOfLine = 0, TypeOfLine = type.ToString(), Stations = l.Stations };
                ret.Add(lp);
            }

            return ret;
        }

        [AllowAnonymous]
        [Route("GetScheduleLines")]
        public IEnumerable<LineStation> GetScheduleLines(string typeOfLine)
        {

            if (typeOfLine == null)
            {
                var type = db.Lines.GetAll().FirstOrDefault(u => u.RouteType == Enums.RouteType.Town);
                //return db.Lines.GetAll().Where(u => u.RouteType == type.RouteType);
                List<Line> lines = db.Lines.GetAll().ToList();
                List<LineStation> ret = new List<LineStation>();

                foreach (Line l in lines)
                {
                    //RouteType type = l.RouteType; //db.TypesOfLine.GetAll().FirstOrDefault(u => u.IDtypeOfLine == l.IDtypeOfLine).typeOfLine;
                    LineStation lp = new LineStation() { Number = l.Number, IDtypeOfLine = 0, TypeOfLine = type.ToString(), Stations = l.Stations };
                    ret.Add(lp);
                }

                return ret;
            }
            else
            {
                //var type = db.Lines.GetAll().FirstOrDefault(u => u.RouteType == (RouteType)typeOfLine);
                RouteType type = Enums.RouteType.Town;
                if (typeOfLine == "Town")
                {
                    type = Enums.RouteType.Town;
                }
                else if (typeOfLine == "Suburban")
                {
                    type = Enums.RouteType.Suburban;
                }
                List<Line> lines = db.Lines.GetAll().ToList();
                List<LineStation> ret = new List<LineStation>();

                foreach (Line l in lines)
                {
                    if (l.RouteType == type)
                    {
                        //RouteType type = l.RouteType; //db.TypesOfLine.GetAll().FirstOrDefault(u => u.IDtypeOfLine == l.IDtypeOfLine).typeOfLine;
                        LineStation lp = new LineStation() { Number = l.Number, IDtypeOfLine = 0, TypeOfLine = type.ToString(), Stations = l.Stations };
                        ret.Add(lp);
                    }
                }

                return ret;
                //return db.Lines.GetAll().Where(u => u.RouteType == type);
            }
        }

        //// GET: api/Lines/5
        [ResponseType(typeof(Line))]
        public IHttpActionResult GetLine(string id)
        {
            List<Line> lines = db.Lines.GetAll().ToList();
            Line line = null;

            foreach (var l in lines)
            {
                if (id == l.Number)
                {
                    line = db.Lines.Get(l.IdLine);
                }
            }

            if (line == null)
            {
                return NotFound();
            }

            return Ok(line);
        }

        [Authorize(Roles = "Admin")]
        [Route("AddLine")]
        public string AddLine(LineStation lineStation)
        {
            Line line = db.Lines.GetAll().FirstOrDefault(u => u.Number == lineStation.Number);


            if (line != null)
            {
                return "Line with that number already exist";
            }
            else
            {
                RouteType id = RouteType.Town;

                if (lineStation.TypeOfLine == "Town")
                {
                    id = Enums.RouteType.Town;
                }
                else if (lineStation.TypeOfLine == "Suburban")
                {
                    id = Enums.RouteType.Suburban;
                }

                //RouteType id = Enums.RouteType.Suburban; //= db.Lines.GetAll().FirstOrDefault(u => u.RouteType == lineStation.TypeOfLine).RouteType;
                Line newLine = new Line() { Number = lineStation.Number, RouteType = id };
                newLine.Stations = new List<Station>();
                foreach (Station s in lineStation.Stations)
                {
                    var station = db.Stations.GetAll().FirstOrDefault(u => u.Name == s.Name);
                    newLine.Stations.Add(station);
                    db.Stations.Update(station);
                }

                db.Lines.Add(newLine);
                try
                {
                    db.Complete();
                }
                catch (Exception e)
                {

                }
            }

            return "ok";
        }

        [Authorize(Roles = "Admin")]
        [Route("EditLine")]
        public string EditLine(LineStation lineStation)
        {
            int result = 1;
            Line line = db.Lines.GetAll().FirstOrDefault(u => u.Number == lineStation.Number);

            if (line == null)
            {
                return "Line can't be changed";
            }
            else
            {
                if (line.Number != lineStation.Number)
                {
                    return "Data was modified in meantime, please try again!";
                }

                //int id = //db.TypesOfLine.GetAll().FirstOrDefault(u => u.typeOfLine == lineStation.TypeOfLine).IDtypeOfLine;

                //line.IdLine = id;

                line.Stations = new List<Station>();
                if (lineStation.Stations != null)
                {
                    foreach (Station s in lineStation.Stations)
                    {
                        var station = db.Stations.GetAll().FirstOrDefault(u => u.Name == s.Name);
                        line.Stations.Add(station);
                        db.Stations.Update(station);
                    }
                }

                if (lineStation.TypeOfLine == "Town")
                {
                    line.RouteType = Enums.RouteType.Town;
                }
                else if (lineStation.TypeOfLine == "Suburban")
                {
                    line.RouteType = Enums.RouteType.Suburban;
                }

                db.Lines.Update(line);
                result = db.Complete();
                if (result == 0)
                {
                    return "conflict";
                }
                else if (result == -1)
                {
                    return "Data was modified in meantime, please try again!";
                }
            }

            return "ok";
        }

        public IHttpActionResult DeleteLine(string Number)
        {
            //Line line = db.Lines.GetAll().FirstOrDefault(u => u.Number == lineStation.Number);

            //Line line = db.Lines.Get(Number);
            List<Line> lines = db.Lines.GetAll().ToList();
            Line line = null;

            foreach (var l in lines)
            {
                if (Number == l.Number)
                {
                    line = db.Lines.Get(l.IdLine);
                }
            }
            if (line == null)
            {
                return NotFound();
            }

            db.Lines.Remove(line);
            db.Complete();

            return Ok(line);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LineExists(string id)
        {
            return db.Lines.GetAll().Count(e => e.Number == id) > 0;

        }
    }
}
