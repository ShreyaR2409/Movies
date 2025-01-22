using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IJwtService
    {
        object GenarateToken(User user, string role, string ApiKey);
    }
}
