using System.Linq;
using Lombiq.Fields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Models;
using Orchard.DynamicForms.Services;
using MediaLibraryUploadField = Lombiq.Fields.Fields.MediaLibraryUploadField;
using MediaLibraryUploadFieldElement = Orchard.DynamicForms.Elements.MediaLibraryUploadField;
using Orchard.DynamicForms.Elements;
using Orchard.DynamicForms.Services.Models;
using Orchard;

namespace Lombiq.Fields.Handlers
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadFieldHandler : ContentHandler, IFormElementEventHandler
    {
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IWorkContextAccessor _wca;

        public MediaLibraryUploadFieldHandler(IContentManager contentManager, IContentDefinitionManager contentDefinitionManager, IWorkContextAccessor wca)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _wca = wca;
        }


        protected override void Loaded(LoadContentContext context)
        {
            base.Loaded(context);

            var fields = context.ContentItem.Parts.SelectMany(x => x.Fields.OfType<MediaLibraryUploadField>());

            if (_contentDefinitionManager.GetTypeDefinition(context.ContentItem.ContentType) == null) return;

            foreach (var field in fields) field.MediaPartsField.Loader(() => _contentManager.GetMany<MediaPart>(field.Ids, VersionOptions.Published, QueryHints.Empty).ToList());
        }

        public void GetElementValue(FormElement element, ReadElementValuesContext context)
        {
            var uploadField = element as MediaLibraryUploadFieldElement;

            if (uploadField == null)
                return;

           // var key = uploadField.Name;
           //// var fileName = context.ValueProvider;
           // context.Output[key] = currentUser != null ? currentUser.UserName : null;
        }

        public void RegisterClientValidation(FormElement element, RegisterClientValidationAttributesContext context) { }
    }
}