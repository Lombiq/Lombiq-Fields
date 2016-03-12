using System.Collections.Generic;
using Lombiq.Fields.Fields;
using Lombiq.Fields.Settings;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Fields;

namespace Lombiq.Fields.ViewModels
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryPickerFieldCarousel")]
    public class MediaLibraryPickerFieldCarouselViewModel
    {
        public ICollection<ContentItem> ContentItems { get; set; }
        public string SelectedIds { get; set; }
        public MediaLibraryPickerField Field { get; set; }
        public ContentPart Part { get; set; }
        public MediaLibraryPickerFieldCarouselSettings Settings { get; set; }
    }
}