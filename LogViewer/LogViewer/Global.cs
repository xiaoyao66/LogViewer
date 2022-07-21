namespace LogViewer
{
    public class Global
    {
        #region Enums
        /// <summary>
        /// 
        /// </summary>
        public enum SearchType
        {
            SubStringCaseInsensitive = 0,
            SubStringCaseSensitive = 1,
            RegexCaseInsensitive = 2,
            RegexCaseSensitive = 3,
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ViewMode
        {
            Standard = 0,
            FilterShow = 1,
            FilterShow2 = 2,
            FilterHide = 4,
            FilterShowShow = FilterShow | FilterShow2,
            FilterHideShow = FilterHide | FilterShow2,
        }
        #endregion
    }
}
