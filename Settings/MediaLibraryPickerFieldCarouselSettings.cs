using Orchard.Environment.Extensions;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryPickerFieldCarousel")]
    public class MediaLibraryPickerFieldCarouselSettings
    {
        public bool IsCarousel { get; set; }
        public bool IsInfinite { get; set; }
        public int ItemsToShow { get; set; }
        public int ItemsToScroll { get; set; }
        public bool IsAutoplay { get; set; }
        public int AutoplaySpeed { get; set; }

        public MediaLibraryPickerFieldCarouselSettings()
        {
            IsCarousel = false;
            IsInfinite = true;
            ItemsToShow = 3;
            ItemsToScroll = 3;
            IsAutoplay = true;
            AutoplaySpeed = 3000;
        }
    }
}