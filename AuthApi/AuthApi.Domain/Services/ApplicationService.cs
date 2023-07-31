using AuthApi.Domain.Interfaces;
using AuthApi.Domain.Repositories;
using AuthApi.Models;
using AuthApi.Models.Application;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Domain.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly AuthApiContext _authApiContext;
        private readonly Repository<Application> _applicationsRepository;
        private readonly Repository<UserApplication> _userApplicationsRepository;


        public ApplicationService(AuthApiContext authApiContext)
        {
            _authApiContext = authApiContext;
            _applicationsRepository = new Repository<Application>(_authApiContext);
            _userApplicationsRepository = new Repository<UserApplication>(_authApiContext);
        }

        public async Task<IEnumerable<Application>> GetApplicationsAsync()
        {
            return await _applicationsRepository.GetAsync();
        }

        public async Task<Application> GetApplicationByGuid(string appId, bool showActive)
        {
            return (showActive) ? await _applicationsRepository.GetSingle(m => m.AppGuid == appId && m.Enabled)
                                : await _applicationsRepository.GetSingle(m => m.AppGuid == appId);
        }

        public async Task<IEnumerable<Application>> GetApplicationsByApplicationName(string applicationName, bool showActive)
        {
            return await _applicationsRepository.Get(m => applicationName.Contains(m.ApplicationName) && m.Enabled == showActive);
        }

        public async Task<Application> CreateApplicationAsync(string applicationName)
        {
            var appModel = new Application
            {
                ApplicationName = applicationName,
                AppGuid = Guid.NewGuid().ToString(),
                Enabled = true,
                CreationDate = DateTime.Now,
            };

            return await _applicationsRepository.InsertAsync(appModel);
        }

        public Task<Application> UpdateApplicationAsync(Application application)
        {
            throw new NotImplementedException();
        }

        public Task<Application> CreateNewApplication(Application application)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateUserApplication(string appGuid, string userName, string email)
        {
            return await (from a in _authApiContext.Users
                          where a.UserName == userName &&
                          a.Email == email &&
                          a.Applications.Any(m => m.AppGuid == appGuid)
                          select a).FirstOrDefaultAsync() != null;


        }

        public async Task AsociateUserApplication(string userId, int appId)
        {

            var newi = new UserApplication
            {
                ApplicationId = appId,
                CreationDate = DateTime.Now,
                Enable = true,
                UserId = userId
            };

            await _userApplicationsRepository.InsertAsync(newi);
        }
    }
}
