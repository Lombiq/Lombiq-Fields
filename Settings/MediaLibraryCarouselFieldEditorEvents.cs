using System.Collections.Generic;
using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryCarouselField")]
    public class MediaLibraryCarouselFieldEditorEvents : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition)
        {

            if (definition.FieldDefinition.Name.Equals(typeof(MediaLibraryCarouselField).Name))
            {
                var model = definition.Settings.GetModel<MediaLibraryCarouselFieldSettings>();
                yield return DefinitionTemplate(model);
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (!builder.FieldType.Equals(typeof(MediaLibraryCarouselField).Name)) yield break;

            var model = new MediaLibraryCarouselFieldSettings();
            if (updateModel.TryUpdateModel(model, typeof(MediaLibraryCarouselFieldSettings).Name, null, null))
            {
                builder.WithSetting("MediaLibraryCarouselFieldSettings.IsCarousel", model.IsCarousel);
            }

            yield return DefinitionTemplate(model);
        }
    }
}