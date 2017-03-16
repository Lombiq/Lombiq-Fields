using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Models;
using System;
using System.Collections.Generic;

namespace Lombiq.Fields.Models
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField.DynamicForms")]
    public class MediaLibraryUploadFieldPostHandlerContext
    {
        public string FileFieldName { get; set; }
        public string AllowedExtensions { get; set; }
        public IUpdateModel Updater { get; set; }
        public int MaximumSizeKB { get; set; }
        public int ImageMaximumWidth { get; set; }
        public int ImageMaximumHeight { get; set; }
        public List<MediaPart> AlreadyUploadedFiles { get; set; }
        public bool Multiple { get; set; }
        public string FolderPath { get; set; }
        public int FieldStorageUserQuotaMB { get; set; }
        public bool Required { get; set; }
        public Action<IEnumerable<int>> StoreIds { get; set; }
    }
}