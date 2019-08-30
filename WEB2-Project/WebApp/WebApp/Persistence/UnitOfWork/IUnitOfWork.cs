using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonRepository PersonRepository { get; set; }
        IPriceRepository Prices { get; set; }
        ILocationRepository Locations { get; set; }
        IPricelistRepository Pricelists { get; set; }
        ILineRepository Lines { get; set; }
        IScheduleRepository Schedules { get; set; }
        IStationRepository Stations { get; set; }
        ITicketRepository Tickets { get; set; }

        int Complete();
    }
}
