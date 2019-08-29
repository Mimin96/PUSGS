using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Unity;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.UnitOfWork
{
    public class DemoUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        [Dependency]
        public ILineRepository Lines { get; set; }

        [Dependency]
        public ILocationRepository Locations { get; set; }

        [Dependency]
        public IPricelistRepository Pricelists { get; set; }

        [Dependency]
        public IPriceRepository Prices { get; set; }

        [Dependency]
        public IScheduleRepository Schedules { get; set; }

        [Dependency]
        public IStationRepository Stations { get; set; }

        [Dependency]
        public ITicketRepository Tickets { get; set; }


        public DemoUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}