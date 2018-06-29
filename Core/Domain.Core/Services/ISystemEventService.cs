namespace Nagnoi.SiC.Domain.Core.Services {
    #region Imports

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents the system event facade
    /// </summary>
    public interface ISystemEventService : IService
    {
        /// <summary>
        /// Retrieves all events by user instance
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="applicationId">Application identifier</param>
        /// <returns>Returns the contents of events</returns>
        IEnumerable<SystemEvent> SelectEventsByUser(User user, int applicationId);

        /// <summary>
        /// Gets the event by its identifier based on the user
        /// </summary>
        /// <param name="user">user instance</param>
        /// <param name="eventId">Event identifier</param>
        /// <returns>Returns the event instance</returns>
        SystemEvent GetEvent(User user, int eventId);

        /// <summary>
        /// Gets the event by its code based on the user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="eventCode">Event code</param>
        /// <returns>Returns the event instance</returns>
        SystemEvent GetEvent(User user, string eventCode);

        /// <summary>
        /// Retrieves all web events
        /// </summary>
        /// <returns>Returns the contents of events</returns>
        IEnumerable<SystemEvent> SelectAllEvents();

        /// <summary>
        /// Validates if the event identifier for current user
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <returns>Returns true or false</returns>
        bool IsValidEvent(int eventId);

        /// <summary>
        /// Retrieves the information about the selected event and assigns it to the CurrentEvent property of the User object
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        void SetCurrentEvent(int eventId);

        /// <summary>
        /// Retrieves the information about the selected event and assigns it to the CurrentEvent property of the User object
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="updateUserInfo">A item indicating whether must update the user information</param>
        void SetCurrentEvent(int eventId, bool updateUserInfo);

        /// <summary>
        /// Impersonate current session with BPP Session.
        /// </summary>
        void ImpersonateFrameworkSession();
    }
}
