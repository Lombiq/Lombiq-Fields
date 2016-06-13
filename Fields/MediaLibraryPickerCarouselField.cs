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
        public bool IsCarousel
        {
            get { return Storage.Get<bool>("IsCarousel"); }
            set { Storage.Set("IsCarousel", value); }
        }


        public bool IsSingleItem
        {
            get { return Storage.Get<bool>("IsSingleItem"); }
            set { Storage.Set("IsSingleItem", value); }
        }


        public bool IsInfinite
        {
            get { return Storage.Get<bool>("IsInfinite"); }
            set { Storage.Set("IsInfinite", value); }
        }


        public int ItemsToShow
        {
            get { return Storage.Get<int>("ItemsToShow"); }
            set { Storage.Set("ItemsToShow", value); }
        }


        public int ItemsToScroll
        {
            get { return Storage.Get<int>("ItemsToScroll"); }
            set { Storage.Set("ItemsToScroll", value); }
        }


        public bool IsAutoplay
        {
            get { return Storage.Get<bool>("IsAutoplay"); }
            set { Storage.Set("IsAutoplay", value); }
        }


        public int AutoplaySpeed
        {
            get { return Storage.Get<int>("AutoplaySpeed"); }
            set { Storage.Set("AutoplaySpeed", value); }
        }
    }
}