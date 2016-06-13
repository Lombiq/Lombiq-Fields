using System;
using System.Globalization;
using System.Linq;
using Lombiq.Fields.Fields;
using Lombiq.Fields.Settings;
using Lombiq.Fields.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.MediaLibrary.Fields;
using Orchard.MediaLibrary.ViewModels;
using Orchard.Utility.Extensions;

namespace Lombiq.Drivers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryPickerFieldCarousel")]
    public class MediaLibraryPickerFieldCarouselDriver : ContentFieldDriver<MediaLibraryPickerCarouselField>
    {
        private readonly IContentManager _contentManager;

        public Localizer T { get; set; }

        public MediaLibraryPickerFieldCarouselDriver(IContentManager contentManager)
        {
            _contentManager = contentManager;

            T = NullLocalizer.Instance;
        }


        private static string GetPrefix(MediaLibraryPickerCarouselField field, ContentPart part)
        {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(MediaLibraryPickerCarouselField field, ContentPart part)
        {
            return field.Name;
        }

        protected override DriverResult Editor(ContentPart part, MediaLibraryPickerCarouselField field, dynamic shapeHelper)
        {
            // If the content item is new, assign the default value.
            if (!part.HasDraft() && !part.HasPublished())
            {
                var settings = part.Fields.Where(x => x.FieldDefinition.Name == typeof(MediaLibraryPickerField).Name).FirstOrDefault().PartFieldDefinition.Settings.GetModel<MediaLibraryPickerFieldCarouselSettings>(); ;
                
                field.IsCarousel = settings.IsCarousel;
                field.IsInfinite = settings.IsInfinite;
                field.ItemsToShow = settings.ItemsToShow;
                field.ItemsToScroll = settings.ItemsToScroll;
                field.IsAutoplay = settings.IsAutoplay;
                field.AutoplaySpeed = settings.AutoplaySpeed;
            }

            return ContentShape("Fields_MediaLibraryPickerCarousel_Edit", GetDifferentiator(field, part),
                () => shapeHelper.EditorTemplate(TemplateName: "Fields/MediaLibraryPickerCarousel.Edit", Model: field, Prefix: GetPrefix(field, part)));
        }

        protected override DriverResult Editor(ContentPart part, MediaLibraryPickerCarouselField field, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(field, GetPrefix(field, part), null, null);

            return Editor(part, field, shapeHelper);
        }
    }
}