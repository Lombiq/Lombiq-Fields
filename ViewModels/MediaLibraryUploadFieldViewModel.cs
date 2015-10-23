using System.Collections.Generic;
using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Lombiq.Fields.ViewModels
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadFieldViewModel
    {
        public ICollection<ContentItem> ContentItems { get; set; }
        public string SelectedIds { get; set; }
        public MediaLibraryUploadField Field { get; set; }
        public ContentPart Part { get; set; }
    }
}