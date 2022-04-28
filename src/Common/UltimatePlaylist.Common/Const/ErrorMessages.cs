namespace UltimatePlaylist.Common.Const
{
    public static class ErrorMessages
    {
        public const string UserDoesNotExist = "User does not exist.";
        public const string EmailConfirmationFailed = "Email confirmation failed.";
        public const string EmailConfirmationExpired = "Email confirmation token has exipred. We have send You new confirmation email. Please check your inbox.";
        public const string UsernameTaken = "This username is taken, please choose another.";
        public const string EmailTaken = "User with given Email already exists.";
        public const string InvalidEmailOrPassword = "Incorrect email address or password.";
        public const string EmailNotConfirmed = "Your email address hasn't been verified. We've sent a new confirmation email, please check your inbox.";
        public const string ResetPasswordFailed = "Reset password failed.";
        public const string ChangePasswordFailed = "Change password failed.";
        public const string RequestToAppleAPIHasFailed = "Request to Apple API has failed.";
        public const string SongDoesNotExist = "Song does not exist. Please contact with support.";
        public const string PlaylistDoesNotExist = "Playlist does not exist. Please contact with support.";
        public const string CannotAddTickets = "The system can not to add tickets to your account. Please contact with support.";
        public const string SongExistInPlaylist = "This song exists in playlist.";
        public const string CannotAddSongToPlaylist = "The system cannot add song to playlist.";
        public const string ProblemWithInitializationPlaylistsInCurrentMonth = "The system has problem with initialization playlists on the currently selected month.";
        public const string CannotSetUserAvatar = "Cannot set user avatar.";
        public const string CannotUpdateUserProfile = "Cannot update user profile.";
        public const string CannotRemoveUserAvatar = "Cannot remove user avatar.";
        public const string UserDoesNotHaveAvatar = "Cannot remove avatar not existing avatar.";
        public const string PinIsIncorrect = "Pin is incorrect. Please try again.";
        public const string CannotSetPin = "Cannot set a new pin.";
        public const string CannotRemovePin = "Cannot remove a pin.";
        public const string FileNotFound = "Cannot find file to upload.";
        public const string FileIsTooLarge = "Cannot upload file because it is too large.";
        public const string WrongFileFormat = "Cannot upload file because file format is not allowed.";
        public const string UserInActive = "The user account has been deactivated by the admin.";
        public const string SkipLimitAlreadyReached = "Skip limit was reached already.";
        public const string LogoutUserInMobileApps = "Email changed succesfully. You have been logged out of the mobile application. Please sign in again.";
        public const string GameNotYetFinished = "Game is not yet finished. Please wait a few seconds and try again.";
        public const string WinningNotFound = "Winning does not exist.";
        public const string UserAgeNeedToVerifiedToChangePaymentStatus = "Please verify age before updating as paid.";
        public const string CouldNotRetrieveUsersStatistics = "Could not retrieve users statistics.";
        public const string UserHasNoTickets = "No tickets avalible for ultimate payout";
        public const string DisconnectedFromAppleMusic = "Your account was disconnected from Apple Music, please connect again from your profile screen.";
        public const string DisconnectedFromSpotify = "Your account was disconnected from Spotify Music, please connect again from your profile screen.";
    }
}
