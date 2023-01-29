using MyTraining1101Demo.Customer;
using MyTraining1101Demo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1101Demo.Migrations.Seed.Host
{
    public class InitialCustomerCreator
    {
            private readonly MyTraining1101DemoDbContext _context;

        public InitialCustomerCreator(MyTraining1101DemoDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var douglas = _context.customers.FirstOrDefault(p => p.EmailId == "douglas.adams@fortytwo.com");
            if (douglas == null)
            {
                _context.customers.Add(
                    new customer
                    {
                        Name = "vishal",
                        Address = "karjat",
                        EmailId = "douglas.adams@fortytwo.com",
                        RegistrationDate= DateTime.Now
                    });
            }

            var asimov = _context.customers.FirstOrDefault(p => p.EmailId == "isaac.asimov@foundation.org");
            if (asimov == null)
            {
                _context.customers.Add(
                    new customer
                    {
                        Name = "vinod",
                        Address = "Ahmednagar",
                        EmailId = "isaac.asimov@foundation.org",
                        RegistrationDate = DateTime.Now
                    });
            }
        }

    }
}
