using System;
using System.Collections.Generic;
using System.Text;
using ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaMirrorTarget;
using ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaProductionSource;
using Microsoft.Extensions.Logging;

namespace ContaVida.DataRefresh.Services
{
    public class DataRefreshService : IDataRefreshService
    {

        private readonly ILogger<DataRefreshService> _logger;
        private ContaVidaDbContextSource _sourceDbContext;
        private ContaVidaDbContextTarget _targetDbContext;

        public DataRefreshService(ILogger<DataRefreshService> logger, 
            ContaVidaDbContextSource sourceDbContext, ContaVidaDbContextTarget targetDbContext)
        {
            _logger = logger;
            _sourceDbContext = sourceDbContext;
            _targetDbContext = targetDbContext;

        }
        public async Task RunDataRefresh()
        {
            _logger.LogWarning("Mirror synchronization started. Turning ON Maintenance page");

            // Sync logic here
            await RefreshUsers();
            await RefreshLogins();

            _logger.LogInformation("Mirror synchronization finished.");
        }

        private async Task RefreshLogins()
        {
            if (_targetDbContext.CorrectLogins.Count() > 0)
            {
                _targetDbContext.CorrectLogins.RemoveRange(_targetDbContext.CorrectLogins);
            }

            foreach (var item in _sourceDbContext.CorrectLogins)
            {
                await _targetDbContext.CorrectLogins.AddAsync(new DataAccess.DataAccess.ContaVidaMirrorTarget.CorrectLogin
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    LoginDate = item.LoginDate,
                    IpAddress = item.IpAddress,
                });
            }

            await _targetDbContext.SaveChangesAsync();
        }

        private async Task RefreshUsers()
        {
            if (_targetDbContext.Users.Count() > 0)
            {
                _targetDbContext.Users.RemoveRange(_targetDbContext.Users);
            }

            foreach (var item in _sourceDbContext.Users)
            {
                await _targetDbContext.Users.AddAsync(new DataAccess.DataAccess.ContaVidaMirrorTarget.User
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    PasswordHash = item.PasswordHash,
                    Salt = item.Salt,
                    AllowSysAdminAccess = item.AllowSysAdminAccess,
                    IsSystemAdmin = item.IsSystemAdmin,
                    CreationDate = item.CreationDate,
                    Email = item.Email,
                    
                });
            }

            await _targetDbContext.SaveChangesAsync();
        }
    }
}
