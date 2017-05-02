using Lombiq.Fields.Elements;
using Lombiq.Fields.Helpers;
using Orchard.DynamicForms.Elements;
using Orchard.DynamicForms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.Fields.Handlers
{
    /// <summary>
    /// This sets the serialized IDs for the content item creation binding.
    /// </summary>
    public class MediaLibraryUploadFieldElementHandler : FormElementEventHandlerBase
    {
        public override void GetElementValue(FormElement element, ReadElementValuesContext context)
        {
            var mediaLibraryUploadField = element as MediaLibraryUploadField;

            if (mediaLibraryUploadField == null)
                return;

            context.Output[mediaLibraryUploadField.Name] =
                MediaLibraryUploadHelper.EncodeIds(mediaLibraryUploadField.Ids);
        }
    }
}