namespace Nagnoi.SiC.FrontEnd.Application.Security
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using Nagnoi.SiC.Domain.Core.Model;
    //using Nagnoi.SiC.Domain.Core.Model.Repository;
    //using Nagnoi.SiC.Domain.Core.Model.Services.Application;
    using Nagnoi.SiC.Infrastructure.Core.Caching;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    using Nagnoi.SiC.Domain.Core.Services;
    using Nagnoi.SiC.Domain.Core.Repositories;
    using Nagnoi.SiC.Domain.Core.Context;

    #endregion

    /// <summary>
    /// Represents the system event facade
    /// </summary>
    public sealed class SystemEventService : ISystemEventService
    {
        #region Constants

        /// <summary>
        /// Key for the Cache of all events
        /// </summary>
        private const string EventsAllCacheDependencyKey = "Nagnoi.SiC.Events.All";

        #endregion

        #region Private Members

        /// <summary>
        /// Customer facade layer reference
        /// </summary>
        private readonly IUserService userService = null;

        /// <summary>
        /// Setting facade
        /// </summary>
        private readonly ISettingService settingService = null;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager cacheManager = null;

        /// <summary>
        /// Framework Authentication Repository
        /// </summary>
        private readonly IFrameworkAuthenticationRepository frameworkAuthenticationRepository = null;

        /// <summary>
        /// System Event Repository
        /// </summary>
        private readonly ISystemEventRepository systemEventRepository = null;

        /// <summary>
        /// Work context reference
        /// </summary>
        private readonly IWorkContext workContext = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegistrationService"/> class
        /// </summary>
        public SystemEventService() : this(
            IoC.Resolve<IFrameworkAuthenticationRepository>(),
            IoC.Resolve<IUserService>(), 
            IoC.Resolve<ISettingService>(),
            IoC.Resolve<ISystemEventRepository>(),
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<IWorkContext>()) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegistrationService"/> class
        /// </summary>
        /// <param name="eventAuthenticationRepository">Authentication Repository layer reference</param>
        /// <param name="userService">Customer Facade layer reference</param>
        /// <param name="messageService">Message Service layer reference</param>
        internal SystemEventService(
            IFrameworkAuthenticationRepository frameworkAuthenticationRepository,
            IUserService userService, 
            ISettingService settingService, 
            ISystemEventRepository systemEventRepository, 
            ICacheManager cacheManager,
            IWorkContext workContext)
        {
            this.frameworkAuthenticationRepository = frameworkAuthenticationRepository;

            this.userService = userService;

            this.settingService = settingService;

            this.systemEventRepository = systemEventRepository;

            this.cacheManager = cacheManager;

            this.workContext = workContext;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a item indicating whether the events cache is enabled
        /// </summary>
        public bool CacheEnabled
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Selects the Events for an user
        /// getting them by merging the framework events and web events
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Returns the contents of events</returns>
        public IEnumerable<SystemEvent> SelectEventsByUser(User user, int applicationId)
        {
            if (user.IsNull())
            {
                throw new ArgumentNullException("user");
            }

            IEnumerable<SystemEvent> effectiveEvents = new List<SystemEvent>();

            IFrameworkSession frameworkSession = IoC.Resolve<IFrameworkSession>();

            // If the user type is Framework then getting the framework events 
            // using the same credentials
            if (user.Type == UserType.Framework)
            {
                effectiveEvents = this.frameworkAuthenticationRepository.SelectEventsByUser(user.LoginName, user.Password);

                var allowedSystemUserEvents = this.frameworkAuthenticationRepository.SelectEventsByUser(frameworkSession.WebEventGlobalBaseUser.Login, frameworkSession.WebEventGlobalBaseUser.Password);

                effectiveEvents = from allowedSystemUserEvent in allowedSystemUserEvents
                                  join effectiveEvent in effectiveEvents
                                  on allowedSystemUserEvent.EventId equals effectiveEvent.EventId
                                  select new SystemEvent()
                                  {
                                      EventId = effectiveEvent.EventId,
                                      Code = effectiveEvent.Code,
                                      Name = effectiveEvent.Name,
                                      StartDate = effectiveEvent.StartDate,
                                      EndDate = effectiveEvent.EndDate,
                                      Id = effectiveEvent.Id
                                  };
            }
            // otherwise using the credentials from System Settings
            else
            {
                IEnumerable<SystemEvent> webEvents = new List<SystemEvent>();
                IEnumerable<SystemEvent> frameworkEvents = this.frameworkAuthenticationRepository.SelectEventsByUser(frameworkSession.WebEventGlobalBaseUser.Login, frameworkSession.WebEventGlobalBaseUser.Password);

                // Get the event contents allowed from Master Web
                webEvents = this.systemEventRepository.SelectEventsByUser(user, applicationId);

                // Find the rest of the event information
                // from matching between web events and framework events
                effectiveEvents = from webEvent in webEvents
                                  join frameworkEvent in frameworkEvents
                                  on webEvent.EventId equals frameworkEvent.EventId
                                  select new SystemEvent()
                                  {
                                      EventId = frameworkEvent.EventId,
                                      Code = frameworkEvent.Code,
                                      Name = frameworkEvent.Name,
                                      StartDate = frameworkEvent.StartDate,
                                      EndDate = frameworkEvent.EndDate,
                                      Id = frameworkEvent.Id
                                  };
            }

            // Multi-event application which use a cookie to determine the event
            string cookieEvent = CookieHelper.GetCookieString("event", true);

            if (effectiveEvents.Any() && !cookieEvent.IsNullOrEmpty())
            {
                effectiveEvents = from systemEvent in effectiveEvents
                                  where systemEvent.Code.Trim().Equals(cookieEvent, StringComparison.OrdinalIgnoreCase)
                                  select systemEvent;
            }

            effectiveEvents = from systemEvent in effectiveEvents
                              orderby systemEvent.EndDate descending
                              select systemEvent;

            return effectiveEvents;
        }

        /// <summary>
        /// Retrieves the information about the selected event and assigns it to the CurrentEvent property of the User object
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        public void SetCurrentEvent(int eventId)
        {
            this.SetCurrentEvent(eventId, true);
        }

        /// <summary>
        /// Retrieves the information about the selected event and assigns it to the CurrentEvent property of the User object
        /// Also retrieves any event-specific info about the User object (e.g CustomerGroupId)
        /// </summary>
        /// <param name="eventId">the Event Id</param>
        /// <param name="storeUserInformation">A item indicating whether must update the user information</param>
        public void SetCurrentEvent(int eventId, bool storeUserInformation)
        {
            // Sets the eventId into the GlobalUsers, 
            // so that any successive calls will point to the correct database for the selected event
            IFrameworkSession frameworkSession = null;

            if (!HttpContext.Current.IsNull() &&
                !HttpContext.Current.Session.IsNull())
            {
                frameworkSession = HttpContext.Current.Session[WorkContextKeys.FrameworkSessionKey] as IFrameworkSession;
            }
            else
            {
                frameworkSession = IoC.Resolve<IFrameworkSession>();
            }

            if (frameworkSession.IsNull())
            {
                frameworkSession = IoC.Resolve<IFrameworkSession>();
            }

            frameworkSession.WebEventGlobalBaseUser.EventID = eventId;
            frameworkSession.WebEventGlobalUser.EventID = eventId;

            if (storeUserInformation)
            {
                // Load the selected event's information 
                // from the corresponding Check-In database
                // and save it to User's event

                // Saves the updated information to CurrentSession before requesting the latest changed event
                HttpContext.Current.Session[WorkContextKeys.FrameworkSessionKey] = frameworkSession;

                // Gets the latest changed event
                this.settingService.Event = IoC.Resolve<ICommonRepository>().GetCurrentEvent();

                SystemEvent selectedEvent = new SystemEvent(this.settingService.Event);
                this.workContext.CurrentUser.CurrentEvent = selectedEvent;

                // Load Event-specific information for the User (e.g. CustomerGroupId)
                // and amend it into the existing user
                User eventSpecificUserInfo = userService.GetUserById(this.workContext.CurrentUser.UserId);
                this.workContext.CurrentUser.OrgId = eventSpecificUserInfo.OrgId;
                this.workContext.CurrentUser.OrgCode = eventSpecificUserInfo.OrgCode;
                this.workContext.CurrentUser.CustomerGroupId = eventSpecificUserInfo.CustomerGroupId;

                IOrganisationService organisationService = IoC.Resolve<IOrganisationService>();
                if (eventSpecificUserInfo.OrgId > 0)
                {
                    this.workContext.CurrentUser.Organisation = organisationService.GetOrganisationById(eventSpecificUserInfo.OrgId);
                }

                if (eventSpecificUserInfo.CustomerGroupId != 0)
                {
                    this.workContext.CurrentUser.Organisations = new List<OrganisationBase>(organisationService.SelectOrganisationsByCustomerGroup(eventSpecificUserInfo.CustomerGroupId));
                    this.workContext.CurrentUser.CustomerGroup = organisationService.GetCustomerGroupByCustomerGroupId(eventSpecificUserInfo.CustomerGroupId);
                }

                this.workContext.CurrentUser.ChainId = eventSpecificUserInfo.ChainId;
                this.workContext.CurrentUser.PreCheckInRef = eventSpecificUserInfo.PreCheckInRef;
                this.workContext.CurrentUser.HotelCode = eventSpecificUserInfo.HotelCode;

                IHotelService hotelService = IoC.Resolve<IHotelService>();
                if (eventSpecificUserInfo.PreCheckInRef > 0 || !eventSpecificUserInfo.HotelCode.IsNullOrEmpty())
                {
                    this.workContext.CurrentUser.Hotel = hotelService.GetHotel(eventSpecificUserInfo.PreCheckInRef, eventSpecificUserInfo.HotelCode);
                }

                if (eventSpecificUserInfo.ChainId != 0)
                {
                    this.workContext.CurrentUser.Hotels = new List<Hotel>(hotelService.SelectHotels(eventSpecificUserInfo.ChainId));

                    var allHotelChains = new List<CheckInObjectLibrary.Setup.TableValue>(IoC.Resolve<IMainModuleService>().SelectHotelChains(eventId));
                    var userChain = allHotelChains.Where(item => item.ValueId == eventSpecificUserInfo.ChainId)
                                                  .FirstOrDefault();

                    if (!userChain.IsNull())
                    {
                        this.workContext.CurrentUser.Chain = userChain;
                    }
                }

                this.workContext.CurrentUser.Profiles = new List<UserProfile>(this.userService.SelectUserProfilesByUserId(this.workContext.CurrentUser.UserId));
            }

            // Save the updated information to CurrentSession
            HttpContext.Current.Session[WorkContextKeys.FrameworkSessionKey] = frameworkSession;
        }

        /// <summary>
        /// Gets the event by its identifier based on the user
        /// </summary>
        /// <param name="user">user instance</param>
        /// <param name="eventId">Event identifier</param>
        /// <returns>Returns the event instance</returns>
        public SystemEvent GetEvent(User user, int eventId)
        {
            if (eventId == 0)
            {
                return null;
            }

            IEnumerable<SystemEvent> events = this.SelectEventsByUser(user, this.settingService.Application.ApplicationId);

            var query = from customEvent in events
                        where customEvent.EventId == eventId
                        select customEvent;

            return query.First();
        }

        /// <summary>
        /// Gets the event by its code based on the user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="eventCode">Event code</param>
        /// <returns>Returns the event instance</returns>
        public SystemEvent GetEvent(User user, string eventCode)
        {
            if (eventCode.IsNullOrEmpty())
            {
                return null;
            }

            IEnumerable<SystemEvent> events = this.SelectEventsByUser(user, this.settingService.Application.ApplicationId);

            return events.Where(item => item.Code.Equals(eventCode, StringComparison.OrdinalIgnoreCase)).First();
        }

        /// <summary>
        /// Validates whether the event identifier for current user
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <returns>Return true or false</returns>
        public bool IsValidEvent(int eventId)
        {
            if (eventId == 0)
            {
                return false;
            }

            if (this.workContext.CurrentUser.IsNull())
            {
                return false;
            }

            if (this.workContext.CurrentUser.Events.IsNull())
            {
                return false;
            }

            if (this.workContext.CurrentUser.Events.IsEmpty())
            {
                return false;
            }

            return this.workContext.CurrentUser.Events.Any(item => item.EventId == eventId);
        }

        /// <summary>
        /// Retrieves all web events
        /// </summary>
        /// <returns>Returns the contents of events</returns>
        public IEnumerable<SystemEvent> SelectAllEvents()
        {
            IEnumerable<SystemEvent> result;

            if (this.CacheEnabled && this.cacheManager.IsAdded(EventsAllCacheDependencyKey))
            {
                Debug.WriteLine("Get Events from Cache");

                result = this.cacheManager.Get(EventsAllCacheDependencyKey) as IEnumerable<SystemEvent>;

                return result.Select(item => (SystemEvent)item.Clone());
            }

            result = this.systemEventRepository.GetAll();

            result = from systemEvent in result
                     where systemEvent.IsActive
                     select systemEvent;

            if (this.CacheEnabled)
            {
                Debug.WriteLine("Insert Events on Cache");

                this.cacheManager.Add(EventsAllCacheDependencyKey, result);
            }

            return result;
        }

        #endregion
    }
}