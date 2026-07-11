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
            await ManageMaintenancePageForProductionEnvironment(setToON: true);

            await RefreshUsers();
            await RefreshLogins();
            await RefreshPersonalProfiles();
            await RefreshPasswordResetRequest();
            await RefreshEventCountersRequest();
            await RefreshRelapses();

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

        private async Task RefreshRelapses()
        {
            if (_targetDbContext.Relapses.Count() > 0)
            {
                var relapses = await _targetDbContext.Relapses.CountAsync();
                _logger.LogWarning("{relapses} relapses found in the Mirror server", relapses);
                _targetDbContext.Relapses.RemoveRange(_targetDbContext.Relapses);
            }

            foreach (var item in _sourceDbContext.Relapses)
            {
                await _targetDbContext.Relapses.AddAsync(new DataAccess.DataAccess.ContaVidaMirrorTarget.Relapse
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    CreationDate = item.CreationDate,
                    EventCounterId = item.EventCounterId,
                    PersonalProfileId = item.PersonalProfileId,
                    PreviousDay = item.PreviousDay,
                    PreviousHour = item.PreviousHour,
                    PreviousMinutes = item.PreviousMinutes,
                    PreviousMonth = item.PreviousMonth,
                    PreviousYear = item.PreviousYear,
                    RelapseDay = item.RelapseDay,
                    RelapseHour = item.RelapseHour,
                    RelapseMinute = item.RelapseMinute,
                    RelapseMonth = item.RelapseMonth,
                    RelapseYear = item.RelapseYear,
                    RelapseMessage = item.RelapseMessage,
                    RelapseReason = item.RelapseReason,
                    

                });
            }

            await _targetDbContext.SaveChangesAsync();
            var relapsesAdded = await _targetDbContext.Relapses.CountAsync();
            _logger.LogInformation("{relapsesAdded} Relapses migrated to Mirror from Production", relapsesAdded);
        }

        private async Task RefreshEventCountersRequest()
        {
            if (_targetDbContext.EventCounters.Count() > 0)
            {
                var eventCounters = await _targetDbContext.EventCounters.CountAsync();
                _logger.LogWarning("{eventCounters} event countets found in the Mirror server", eventCounters);
                _targetDbContext.EventCounters.RemoveRange(_targetDbContext.EventCounters);
            }

            foreach (var item in _sourceDbContext.EventCounters)
            {
                await _targetDbContext.EventCounters.AddAsync(new DataAccess.DataAccess.ContaVidaMirrorTarget.EventCounter
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    CreationDate = item.CreationDate,
                    CustomMessage = item.CustomMessage,
                    EventName = item.EventName,
                    IsPublic = item.IsPublic,
                    Hour = item.Hour,
                    Minutes = item.Minutes,
                    PersonalProfileId = item.PersonalProfileId,
                    RefreshMinutesTime = item.RefreshMinutesTime,
                    CustomEventImageCollection = item.CustomEventImageCollection,
                    StartDay = item.StartDay,
                    StartMonth = item.StartMonth,
                    StartYear = item.StartYear,
                    Status = item.Status,
                });
            }

            await _targetDbContext.SaveChangesAsync();
            var migratedEvents = await _targetDbContext.EventCounters.CountAsync();
            _logger.LogInformation("{migratedEvents} counter events migrated to Mirror from Production", migratedEvents);
        }

        private async Task RefreshPasswordResetRequest()
        {
            if (_targetDbContext.ResetLoginPasswords.Count() > 0)
            {
                var resetLoginPassword = await _targetDbContext.ResetLoginPasswords.CountAsync();
                _logger.LogWarning("{resetLoginPassword} reset login requests found in the Mirror server", resetLoginPassword);
                _targetDbContext.ResetLoginPasswords.RemoveRange(_targetDbContext.ResetLoginPasswords);
            }

            foreach (var item in _sourceDbContext.ResetLoginPasswords)
            {
                await _targetDbContext.ResetLoginPasswords.AddAsync(new DataAccess.DataAccess.ContaVidaMirrorTarget.ResetLoginPassword
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    CreationDate = item.CreationDate,
                    ExpirationDate = item.ExpirationDate,
                });
            }

            await _targetDbContext.SaveChangesAsync();
            var migratedResetLoginPasswords = await _targetDbContext.ResetLoginPasswords.CountAsync();
            _logger.LogInformation("{migratedResetLoginPasswords} Password reset requests migrated to Mirror from Production", migratedResetLoginPasswords);
        }

        private async Task RefreshPersonalProfiles()
        {
            if (_targetDbContext.PersonalProfiles.Count() > 0)
            {
                var profiles = await _targetDbContext.PersonalProfiles.CountAsync();
                _logger.LogWarning("{profiles} history logins found in the Mirror server", profiles);
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
            _logger.LogInformation("{migratedPersonalProfiles} Personal profiles migrated to Mirror from Production", migratedPersonalProfiles);
        }

        private async Task RefreshLogins()
        {
            if (_targetDbContext.CorrectLogins.Count() > 0)
            {
                var totalLogins = await _targetDbContext.CorrectLogins.CountAsync();
                _logger.LogWarning("{totalLogins} history logins found in the Mirror server", totalLogins);
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
            var migratedLogins = await _targetDbContext.CorrectLogins.CountAsync();
            _logger.LogInformation("{migratedUsers} Logins migrated to Mirror from Production", migratedLogins);
        }

        private async Task RefreshUsers()
        {
            if (_targetDbContext.Users.Count() > 0)
            {
                var totalUsers = await _targetDbContext.Users.CountAsync();
                _logger.LogWarning("{totalUsers} users found in the Mirror server", totalUsers);
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
