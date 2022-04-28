#region Usings

using System.Runtime.Serialization;

#endregion

namespace UltimatePlaylist.Common.Enums
{
    public enum EventGridEventType
    {
        None = 0,

        /// <summary>
        /// Get an event when Job transitions to scheduled state.
        /// </summary>
        [EnumMember(Value = "Microsoft.Media.JobScheduled")]
        MediaServicesJobScheduled,

        /// <summary>
        /// Get an event when Job transitions to processing state.
        /// </summary>
        [EnumMember(Value = "Microsoft.Media.JobProcessing")]
        MediaServicesJobProcessing,

        /// <summary>
        /// Get an event when Job transitions to finished state. This is a final state that includes Job outputs.
        /// </summary>
        [EnumMember(Value = "Microsoft.Media.JobFinished")]
        MediaServicesJobFinished,

        /// <summary>
        /// Get an event when Job transitions to canceled state. This is a final state that includes Job outputs.
        /// </summary>
        [EnumMember(Value = "Microsoft.Media.JobCanceled")]
        MediaServicesJobCanceled,

        /// <summary>
        /// Get an event when Job transitions to error state. This is a final state that includes Job outputs.
        /// </summary>
        [EnumMember(Value = "Microsoft.Media.JobErrored")]
        MediaServicesJobErrored,

        /// <summary>
        /// Get an event when existing Event Grid subscription is deleted.
        /// </summary>
        [EnumMember(Value = "Microsoft.EventGrid.SubscriptionDeletedEvent")]
        EventGridSubscriptionDeleted,

        /// <summary>
        /// Get an event when new Event Grid subscription is created and it's ready to be validated.
        /// </summary>
        [EnumMember(Value = "Microsoft.EventGrid.SubscriptionValidationEvent")]
        EventGridSubscriptionValidation,
    }
}
