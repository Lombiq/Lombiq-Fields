using System.Collections.Generic;
using System.Globalization;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Fields;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryPickerFieldCarousel")]
    public class MediaLibraryPickerFieldCarouselEditorEvents : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition)
        {
            if (definition.FieldDefinition.Name.Equals(typeof(MediaLibraryPickerField).Name))
            {
                var model = definition.Settings.GetModel<MediaLibraryPickerFieldCarouselSettings>();
                yield return DefinitionTemplate(model);
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (!builder.FieldType.Equals(typeof(MediaLibraryPickerField).Name)) yield break;

            var model = new MediaLibraryPickerFieldCarouselSettings();
            if (updateModel.TryUpdateModel(model, typeof(MediaLibraryPickerFieldCarouselSettings).Name, null, null))
            {
                builder.WithSetting("MediaLibraryPickerFieldCarouselSettings.IsCarousel", model.IsCarousel.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("MediaLibraryPickerFieldCarouselSettings.IsInfinite", model.IsInfinite.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("MediaLibraryPickerFieldCarouselSettings.ItemsToShow", model.ItemsToShow.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("MediaLibraryPickerFieldCarouselSettings.ItemsToScroll", model.ItemsToScroll.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("MediaLibraryPickerFieldCarouselSettings.IsAutoplay", model.IsAutoplay.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("MediaLibraryPickerFieldCarouselSettings.AutoplaySpeed", model.AutoplaySpeed.ToString(CultureInfo.InvariantCulture));
            }

            yield return DefinitionTemplate(model);
        }
    }
}