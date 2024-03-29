﻿namespace UltimatePlaylist.Common.Enums
{
    public enum MediaServicesJobErrorCode
    {
        /// <summary>
        /// There was a problem with the combination of input files
        /// and the configuration settings applied, fix the configuration settings
        /// and retry with the same input, or change input to match the configuration.
        /// </summary>
        ConfigurationUnsupported = 0,

        /// <summary>
        /// There was a problem with the input content
        /// (for example: zero byte files, or corrupt/non-decodable files), check the input files.
        /// </summary>
        ContentMalformed,

        /// <summary>
        /// There was a problem with the format of the input
        /// (not valid media file, or an unsupported file/codec), check the validity of the input files.
        /// </summary>
        ContentUnsupported,

        /// <summary>
        /// While trying to download the input files, the files were not accessible,
        /// please check the availability of the source.
        /// </summary>
        DownloadNotAccessible,

        /// <summary>
        /// While trying to download the input files, there was an issue during transfer
        /// (storage service, network errors), see details and check your source.
        /// </summary>
        DownloadTransientError,

        /// <summary>
        /// Fatal service error, please contact support.
        /// </summary>
        ServiceError,

        /// <summary>
        /// Transient error, please retry, if retry is unsuccessful, please contact support.
        /// </summary>
        ServiceTransientError,

        /// <summary>
        /// While trying to upload the output files, the destination was not reachable,
        /// please check the availability of the destination.
        /// </summary>
        UploadNotAccessible,

        /// <summary>
        /// While trying to upload the output files, there was an issue during transfer
        /// (storage service, network errors), see details and check your destination.
        /// </summary>
        UploadTransientError,
    }
}
