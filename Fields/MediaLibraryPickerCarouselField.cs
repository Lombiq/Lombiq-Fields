using System;
using Lombiq.Fields.Settings;
using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Fields;

namespace Lombiq.Fields.Fields
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryPickerFieldCarousel")]
    public class MediaLibraryPickerCarouselField : ContentField
    {
        private static readonly char[] _separators = new[] { '{', '}', ',' };
        public MediaLibraryPickerFieldCarouselSettings Settings
        {
            get { return DecodeSettings(Storage.Get<string>()); }
            set { Storage.Set(EncodeSettings(value)); }
        }

        private string EncodeSettings(MediaLibraryPickerFieldCarouselSettings settings)
        {
            if (settings == null)
            {
                return string.Empty;
            }

            return "{" + settings.IsCarousel.ToString() + "},{"
                + settings.IsSingleItem.ToString() + "},{"
                + settings.IsInfinite.ToString() + "},{"
                + settings.ItemsToShow.ToString() + "},{"
                + settings.ItemsToScroll.ToString() + "},{"
                + settings.IsAutoplay.ToString() + "},{"
                + settings.AutoplaySpeed.ToString() + "}";
        }

        private MediaLibraryPickerFieldCarouselSettings DecodeSettings(string settings)
        {
            if (String.IsNullOrWhiteSpace(settings))
            {
                return null;
            }

            var storedSettings = settings.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
            var carouselSettings = new MediaLibraryPickerFieldCarouselSettings();

            carouselSettings.IsCarousel = bool.Parse(storedSettings[0]);
            carouselSettings.IsSingleItem = bool.Parse(storedSettings[1]);
            carouselSettings.IsInfinite = bool.Parse(storedSettings[2]);
            carouselSettings.ItemsToShow = int.Parse(storedSettings[3]);
            carouselSettings.ItemsToScroll = int.Parse(storedSettings[4]);
            carouselSettings.IsAutoplay = bool.Parse(storedSettings[5]);
            carouselSettings.AutoplaySpeed = int.Parse(storedSettings[6]);

            return carouselSettings;
        }
    }
}