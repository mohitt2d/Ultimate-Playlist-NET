namespace UltimatePlaylist.Common.Enums
{
    public enum MediaServicesJobErrorCategoryType
    {
        /// <summary>
        /// The error is configuration related.
        /// </summary>
        Configuration = 0,

        /// <summary>
        /// The error is related to data in the input files.
        /// </summary>
        Content,

        /// <summary>
        /// The error is download related.
        /// </summary>
        Download,

        /// <summary>
        /// The error is service related.
        /// </summary>
        Service,

        /// <summary>
        /// The error is upload related.
        /// </summary>
        Upload,
    }
}
