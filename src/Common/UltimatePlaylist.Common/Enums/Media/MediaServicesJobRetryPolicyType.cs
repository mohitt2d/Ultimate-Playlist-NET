namespace UltimatePlaylist.Common.Enums
{
    public enum MediaServicesJobRetryPolicyType
    {
        /// <summary>
        /// Issue needs to be investigated and then the job resubmitted
        /// with corrections or retried once the underlying issue has been corrected.
        /// </summary>
        DoNotRetry = 0,

        /// <summary>
        /// Issue may be resolved after waiting for a period of time and resubmitting the same Job.
        /// </summary>
        MayRetry,
    }
}
