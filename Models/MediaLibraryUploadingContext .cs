using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Models;
using System;
using System.Collections.Generic;

namespace Lombiq.Fields.Models
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField.DynamicForms")]
    public class MediaLibraryUploadingContext
    {
        public IMediaLibraryUploadSettings MediaLibraryUploadSettings { get; set; }
        public string FileFieldName { get; set; }
        public IUpdateModel Updater { get; set; }
        public List<MediaPart> AlreadyUploadedFiles { get; set; }
        public string FolderPath { get; set; }
        public Action<IEnumerable<int>> StoreIds { get; set; }
    }
}