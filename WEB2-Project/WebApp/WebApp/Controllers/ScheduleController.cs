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
    [RoutePrefix("api/Schedule")]
    public class ScheduleController : ApiController
    {
        private IUnitOfWork db;
        public ScheduleController(IUnitOfWork db)
        {
            this.db = db;
        }

        [Authorize(Roles = "Admin")]
        [Route("PostLineSchedule")]
        // POST: api/Schedules
        [ResponseType(typeof(Schedule))]
        public IHttpActionResult PostLineSchedule([FromBody]ScheduleLine sl)
        {
            //ne radi dobro izmeniti

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (sl == null)
            {
                return NotFound();
            }

            DayType dd = DayType.Workday;
            if (sl.Day == "Work day")
            {
                dd = Enums.DayType.Workday;
            }
            else if (sl.Day == "Weekend")
            {
                dd = Enums.DayType.Weekend;
            }

            Schedule d = new Schedule { Day = dd, DepartureTime = sl.Time.ToString() };
            if (d.Lines == null)
            {
                d.Lines = new List<Line>();
            }
            var line = db.Lines.GetAll().FirstOrDefault(u => u.Number == sl.Number);
            if (line.Stations == null)
            {
                line.Stations = new List<Station>();
            }

            Schedule exist = db.Schedules.GetAll().FirstOrDefault(u => (u.DepartureTime == sl.Time.ToString() && u.Day == dd));
            if (exist == null)
            {

                d.Lines.Add(line);
                d.Line = line;
                db.Schedules.Add(d);
                line.Schedules.Add(d);
                db.Lines.Update(line);
            }
            else
            {
                if (line.Schedules.FirstOrDefault(u => (u.DepartureTime == sl.Time.ToString() && u.Day == dd)) == null)
                {
                    exist.Lines.Add(line);
                    db.Schedules.Update(exist);
                    line.Schedules.Add(exist);
                    db.Lines.Update(line);
                }
            }

            db.Complete();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("EditLineSchedule")]
        // POST: api/Schedules
        [ResponseType(typeof(Schedule))]
        public IHttpActionResult EditLineSchedule([FromBody]ScheduleLine sl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (sl == null)
            {
                return NotFound();
            }

            DayType dd = DayType.Workday;
            if (sl.Day == "Work day")
            {
                dd = Enums.DayType.Workday;
            }
            else if (sl.Day == "Weekend")
            {
                dd = Enums.DayType.Weekend;
            }

            Schedule s = new Schedule { Day = dd, DepartureTime = sl.Time.ToString() };
            var line = db.Lines.GetAll().FirstOrDefault(u => u.Number == sl.Number);

            if (s.Lines == null)
            {
                s.Lines = new List<Line>();
            }

            if (sl.Number != "")
            {
                s.Lines.Add(line);
                s.Line = line;
                //s.IdLine = line.IdLine;
                s.Type = line.RouteType;
            }

            List<Schedule> Schedules = db.Schedules.GetAll().ToList();
            Schedule scheduleFromBase = null;

            foreach (var sc in Schedules)
            {
                if (sc.Lines != null)
                {
                    foreach (var l in sc.Lines)
                    {
                        if (l.Number == sl.Number)
                        {
                            scheduleFromBase = sc;
                        }
                    }
                }

            }

            

            if (scheduleFromBase.Lines.Count == 1)
            {
                Schedule exist = db.Schedules.GetAll().FirstOrDefault(u => (u.DepartureTime == sl.Time.ToString()/*&& u.IdSchadule == sl.IDDay*/));
                if (exist == null)
                {
                    scheduleFromBase.DepartureTime = sl.Time.ToString();
                    scheduleFromBase.Day = dd;
                    db.Schedules.Update(scheduleFromBase);

                    for (int i = 0; i < line.Schedules.Count; i++)
                    {
                        if (line.Schedules[i].IdSchadule == scheduleFromBase.IdSchadule)
                        {
                            line.Schedules[i] = scheduleFromBase;
                        }
                    }

                    db.Lines.Update(line);
                }
                else
                {
                    db.Schedules.Remove(scheduleFromBase);
                    s.Lines.Add(line);
                    db.Schedules.Update(s);
                    line.Schedules.Remove(scheduleFromBase);
                    line.Schedules.Add(s);
                    db.Lines.Update(line);

                }

            }
            else if (scheduleFromBase.Lines.Count > 1)
            {
                Schedule exist = db.Schedules.GetAll().FirstOrDefault(u => (u.DepartureTime == sl.Time.ToString() && u.Day == dd));
                if (exist == null)
                {

                    scheduleFromBase.Lines.Remove(line);
                    line.Schedules.Remove(scheduleFromBase);
                    s.Lines.Add(line);
                    db.Schedules.Add(s);
                    line.Schedules.Add(s);
                    db.Lines.Update(line);
                }
                else
                {
                    scheduleFromBase.Lines.Remove(line);
                    line.Schedules.Remove(scheduleFromBase);
                    exist.Lines.Add(line);
                    db.Schedules.Update(exist);
                    line.Schedules.Add(exist);
                    db.Lines.Update(line);
                }
            }
            int r = 1;
            r = db.Complete();
            if (r == -1)
            {
                return BadRequest("bad");
            }


            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("DeleteLineSchedule/{Number}/{Day}")]
        // DELETE: api/Schedules/5
        [ResponseType(typeof(Schedule))]
        public IHttpActionResult DeleteLineSchedule(string Number, string Day)
        {
            if (Number == null)
            {
                return NotFound();
            }

            List<Schedule> schedules = db.Schedules.GetAll().ToList();

            List<Line> lines = db.Lines.GetAll().ToList();
            Line line = null;
            Schedule schedule = null;

            DayType dd = DayType.Workday;
            if (Day == "Work day")
            {
                dd = DayType.Workday;
            }
            else if (Day == "Weekend")
            {
                dd = DayType.Weekend;
            }

            foreach (var s in schedules)
            {
                if (s.Lines != null)
                {
                    foreach (var l in s.Lines)
                    {
                        if (Number == l.Number && s.Day == dd)
                        {
                            line = db.Lines.Get(l.IdLine);
                            schedule = db.Schedules.Get(s.IdSchadule);
                        }
                    }
                }
            }
            if (schedule == null)
            {
                return NotFound();
            }

            line.Schedules.Remove(schedule);
            db.Lines.Update(line);
            schedule.Lines.Remove(line);
            db.Schedules.Update(schedule);
            db.Complete();

            return Ok(schedule);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SchaduleExists(int id)
        {
            return db.Schedules.GetAll().Count(e => e.IdSchadule == id) > 0;
        }
    }
}
