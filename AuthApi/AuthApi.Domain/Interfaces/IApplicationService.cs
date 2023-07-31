using AuthApi.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Domain.Interfaces
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetApplicationsAsync();
        Task<Application> GetApplicationByGuid(string appId, bool showActive);
        Task<IEnumerable<Application>> GetApplicationsByApplicationName(string applicationName, bool showActive);
        Task<Application> CreateApplicationAsync(string applicationName);
        Task<Application> UpdateApplicationAsync(Application application);
        Task<bool> ValidateUserApplication(string appId, string userName, string email);
        Task AsociateUserApplication(string userId, int appId);
    }
}
