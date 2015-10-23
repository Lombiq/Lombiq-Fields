using Orchard.Environment.Extensions;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadFieldSettings
    {
        #region General field settings
        public string Hint { get; set; }
        public bool Required { get; set; }
        public bool Multiple { get; set; }
        public string AllowedExtensions { get; set; }
        public string FolderPath { get; set; }        
        #endregion

        #region Image-specific settings
        public int ImageMaximumSize { get; set; }
        public int ImageMaximumWidth { get; set; }
        public int ImageMaximumHeight { get; set; }
        public string MediaProfile { get; set; }
        #endregion
    }
}