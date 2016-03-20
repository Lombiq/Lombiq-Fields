using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Fields;
using System.Linq;

namespace Lombiq.Fields.Handlers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryPickerFieldCarousel")]
    public class MediaLibraryPickerFieldCarouselHandler : ContentHandler
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public MediaLibraryPickerFieldCarouselHandler(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        protected override void Initializing(InitializingContentContext context)
        {
            base.Initializing(context);

            var partsWithMediaLibraryPickerField = context.ContentItem.Parts.SelectMany(x => x.Fields.OfType<MediaLibraryPickerField>());
            if (partsWithMediaLibraryPickerField.Any())
            {
                var partsWithCarouselField = context.ContentItem.Parts.SelectMany(x => x.Fields.OfType<MediaLibraryPickerCarouselField>());
                if (!partsWithCarouselField.Any())
                {
                    _contentDefinitionManager.AlterPartDefinition(context.ContentItem.Parts.Where(
                        x => x.PartDefinition.Name == context.ContentItem.ContentType).FirstOrDefault()
                        .TypePartDefinition.PartDefinition.Name,
                        cfg => cfg.WithField(typeof(MediaLibraryPickerCarouselField).Name,
                            f => f.OfType(typeof(MediaLibraryPickerCarouselField).Name)
                                .WithDisplayName(typeof(MediaLibraryPickerCarouselField).Name)));
                }
            }
        }
    }
}