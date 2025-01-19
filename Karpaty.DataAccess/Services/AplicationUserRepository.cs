using Karpaty.Data;
using Karpaty.DataAccess.Services.IServices;
using Karpaty.Models;
using Karpaty.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karpaty.DataAccess.Services
{
    public class AplicationUserRepository:Repository<AplicationUser>, IAplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public AplicationUserRepository(ApplicationDbContext db): base(db) 
        {
            _db=db;
        }
    }
}
