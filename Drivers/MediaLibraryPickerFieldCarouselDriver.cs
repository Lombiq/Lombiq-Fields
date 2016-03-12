using System;
using System.Linq;
using Lombiq.Fields.Settings;
using Lombiq.Fields.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.MediaLibrary.Fields;
using Orchard.MediaLibrary.Settings;
using Orchard.MediaLibrary.ViewModels;
using Orchard.Utility.Extensions;

namespace Lombiq.Drivers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryPickerFieldCarousel")]
    public class MediaLibraryPickerFieldCarouselDriver : ContentFieldDriver<MediaLibraryPickerField>
    {
        private readonly IContentManager _contentManager;

        public Localizer T { get; set; }

        public MediaLibraryPickerFieldCarouselDriver(IContentManager contentManager)
        {
            _contentManager = contentManager;

            T = NullLocalizer.Instance;
        }


        private static string GetPrefix(MediaLibraryPickerField field, ContentPart part)
        {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(MediaLibraryPickerField field, ContentPart part)
        {
            return field.Name;
        }

        protected override DriverResult Editor(ContentPart part, MediaLibraryPickerField field, dynamic shapeHelper)
        {
            return ContentShape("Fields_MediaLibraryPicker_Edit", GetDifferentiator(field, part),
                () => {
                    var model = new MediaLibraryPickerFieldCarouselViewModel
                    {
                        Field = field,
                        Part = part,
                        ContentItems = _contentManager.GetMany<ContentItem>(field.Ids, VersionOptions.Published, QueryHints.Empty).ToList(),
                        Settings = field.PartFieldDefinition.Settings.GetModel<MediaLibraryPickerFieldCarouselSettings>()
                    };

                    model.SelectedIds = string.Concat(",", field.Ids);

                    return shapeHelper.EditorTemplate(TemplateName: "Fields/MediaLibraryPickerFieldCarousel.Edit", Model: model, Prefix: GetPrefix(field, part));
                });
        }

        protected override DriverResult Editor(ContentPart part, MediaLibraryPickerField field, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = new MediaLibraryPickerFieldCarouselViewModel { SelectedIds = string.Join(",", field.Ids) };

            updater.TryUpdateModel(model, GetPrefix(field, part), null, null);

            // Update somehow

            return Editor(part, field, shapeHelper);
        }
    }
}