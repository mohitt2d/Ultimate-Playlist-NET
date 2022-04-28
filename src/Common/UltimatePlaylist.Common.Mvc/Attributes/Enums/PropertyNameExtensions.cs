namespace UltimatePlaylist.Common.Mvc.Attributes.Enums
{
    public static class PropertyNameExtensions
    {
        #region Public Methods

        public static string GetName(this PropertyName name)
        {
            return PropertyNames.ResourceManager.GetString(name.ToString())
                                 ?? name.ToString();
        }

        #endregion
    }
}
