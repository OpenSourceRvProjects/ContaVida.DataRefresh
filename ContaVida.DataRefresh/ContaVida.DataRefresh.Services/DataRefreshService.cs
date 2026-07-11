using System;
using System.Collections.Generic;
using System.Text;
using ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaMirrorTarget;
using ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaProductionSource;
using Microsoft.EntityFrameworkCore;
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

            // Sync logic here
            await ManageMaintenancePageForProductionEnvironment( setToON: true );
            await RefreshUsers();
            await RefreshLogins();
            await RefreshPersonalProfiles();

            await ManageMaintenancePageForProductionEnvironment(setToON: false);
        }

        private async Task ManageMaintenancePageForProductionEnvironment(bool setToON)
        {
            string status = setToON ? "ON" : "OFF";
            _logger.LogWarning("Turning {status} Maintenance page", status);
            var sourceMaintenancePage = _sourceDbContext.SystemMaintenances.OrderBy(o => o.Id).FirstOrDefault();
            sourceMaintenancePage.IsOnMaintenance = setToON;
            await _sourceDbContext.SaveChangesAsync();
        }


        private async Task RefreshPersonalProfiles()
        {
            if (_targetDbContext.PersonalProfiles.Count() > 0)
            {
                var profiles = await _targetDbContext.PersonalProfiles.CountAsync();
                _logger.LogWarning("Preparing to sync {profiles} history logins to target DB", profiles);
                _targetDbContext.PersonalProfiles.RemoveRange(_targetDbContext.PersonalProfiles);
            }

            foreach (var item in _sourceDbContext.PersonalProfiles)
            {
                await _targetDbContext.PersonalProfiles.AddAsync(new DataAccess.DataAccess.ContaVidaMirrorTarget.PersonalProfile
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    Address = item.Address,
                    CounterLimit = item.CounterLimit,
                    CreationDate = item.CreationDate,
                    DefaultPetPhotos = item.DefaultPetPhotos,
                    LastName1 = item.LastName1,
                    LastName2 = item.LastName2,
                    Name = item.Name,
                    Pohone = item.Pohone,
                    RelapseLimit = item.RelapseLimit,
                    
                });
            }

            await _targetDbContext.SaveChangesAsync();
            var migratedPersonalProfiles = await _targetDbContext.PersonalProfiles.CountAsync();
            _logger.LogInformation("{migratedPersonalProfiles} migrated to Mirror from Production", migratedPersonalProfiles);
        }


        private async Task RefreshLogins()
        {
            if (_targetDbContext.CorrectLogins.Count() > 0)
            {
                var totalLogins = await _targetDbContext.CorrectLogins.CountAsync();
                _logger.LogWarning("Preparing to sync {totalLogins} history logins to target DB", totalLogins);
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
            var migratedLogins= await _targetDbContext.CorrectLogins.CountAsync();
            _logger.LogInformation("{migratedUsers} logins migrated to Mirror from Production", migratedLogins);
        }

        private async Task RefreshUsers()
        {
            if (_targetDbContext.Users.Count() > 0)
            {
                var totalUsers = await _targetDbContext.Users.CountAsync();
                _logger.LogWarning("Preparing to sync {totalUsers} users to target DB", totalUsers);
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
            var migratedUsers = await _targetDbContext.Users.CountAsync();
            _logger.LogInformation("{migratedUsers} Users migrated to Mirror from Production", migratedUsers);

        }
    }
}
