using System.Collections.Generic;
using System.Globalization;
using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;

namespace Lombiq.Fields.Settings
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadFieldEditorEvents : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition)
        {
            if (definition.FieldDefinition.Name.Equals(typeof(MediaLibraryUploadField).Name))
            {
                var model = definition.Settings.GetModel<MediaLibraryUploadFieldSettings>();
                yield return DefinitionTemplate(model);
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (!builder.FieldType.Equals(typeof(MediaLibraryUploadField).Name)) yield break;

            var model = new MediaLibraryUploadFieldSettings();
            if (updateModel.TryUpdateModel(model, typeof(MediaLibraryUploadFieldSettings).Name, null, null))
            {
                builder.WithSetting("MediaLibraryUploadFieldSettings.Hint", model.Hint);
                builder.WithSetting("MediaLibraryUploadFieldSettings.Required", model.Required.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("MediaLibraryUploadFieldSettings.Multiple", model.Multiple.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("MediaLibraryUploadFieldSettings.AllowedExtensions", model.AllowedExtensions);
                builder.WithSetting("MediaLibraryUploadFieldSettings.FolderPath", model.FolderPath);
                builder.WithSetting("MediaLibraryUploadFieldSettings.ImageMaximumSize", model.ImageMaximumSize.ToString());
                builder.WithSetting("MediaLibraryUploadFieldSettings.ImageMaximumWidth", model.ImageMaximumWidth.ToString());
                builder.WithSetting("MediaLibraryUploadFieldSettings.ImageMaximumHeight", model.ImageMaximumHeight.ToString());
                builder.WithSetting("MediaLibraryUploadFieldSettings.MediaProfile", model.MediaProfile);
            }

            yield return DefinitionTemplate(model);
        }
    }
}