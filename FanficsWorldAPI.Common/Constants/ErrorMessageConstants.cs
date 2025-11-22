namespace FanficsWorldAPI.Common.Constants
{
    public static class ErrorMessageConstants
    {
        public const string InvalidPassword = "Wrong password. Please try again.";

        public const string UserNotFound = "User was not found.";

        public const string UserInactive = "User is not available due to lockout.";

        public const string Forbidden = "You do not have permission to perform this action.";

        public const string FanficNotFound = "Fanfic was not found.";

        public const string CannotDeleteFanficChaptersNotDeleted = "Cannot delete a fanfic - the fanfic's chapters has not been deleted.";
    }
}
