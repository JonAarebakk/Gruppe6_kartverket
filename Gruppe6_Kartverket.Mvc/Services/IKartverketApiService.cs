using Gruppe6_Kartverket.Mvc.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruppe6_Kartverket.Mvc.Services
{
    public interface IKartverketApiService
    {
        Task<KartverkApiInfo> GetMunicipalityAndCountyNameAsync(double longitude, double latitude);
    }
}
