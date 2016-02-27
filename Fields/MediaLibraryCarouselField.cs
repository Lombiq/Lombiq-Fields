using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage;
using Orchard.Environment.Extensions;

namespace Lombiq.Fields.Fields
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryCarouselField")]
    public class MediaLibraryCarouselField : ContentField
    {
        public bool? IsCarousel
        {
            get { return Storage.Get<bool?>(); }

            set { Storage.Set(value); }
        }
    }
}