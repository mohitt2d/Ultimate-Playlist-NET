﻿namespace UltimatePlaylist.Common.Enums
{
    public enum MediaServicesJobState
    {
        None = 0,

        /// <summary>
        /// The job was canceled. This is a final state for the job.
        /// </summary>
        Canceled,

        /// <summary>
        /// The job is in the process of being canceled.
        /// This is a transient state for the job.
        /// </summary>
        Canceling,

        /// <summary>
        /// The job has encountered an error. This is a final state for the job.
        /// </summary>
        Error,

        /// <summary>
        /// The job is finished. This is a final state for the job.
        /// </summary>
        Finished,

        /// <summary>
        /// The job is processing. This is a transient state for the job.
        /// </summary>
        Processing,

        /// <summary>
        /// The job is in a queued state, waiting for resources to become available.
        /// This is a transient state.
        /// </summary>
        Queued,

        /// <summary>
        /// The job is being scheduled to run on an available resource.
        /// This is a transient state, between queued and processing states.
        /// </summary>
        Scheduled,
    }
}
