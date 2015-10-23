using System.Linq;
using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Models;

namespace Lombiq.Fields.Handlers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadFieldHandler : ContentHandler
    {
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;


        public MediaLibraryUploadFieldHandler(IContentManager contentManager, IContentDefinitionManager contentDefinitionManager)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
        }


        protected override void Loaded(LoadContentContext context)
        {
            base.Loaded(context);

            var fields = context.ContentItem.Parts.SelectMany(x => x.Fields.OfType<MediaLibraryUploadField>());

            if (_contentDefinitionManager.GetTypeDefinition(context.ContentItem.ContentType) == null) return;

            foreach (var field in fields) field.MediaPartsField.Loader(() => _contentManager.GetMany<MediaPart>(field.Ids, VersionOptions.Published, QueryHints.Empty).ToList());
        }
    }
}