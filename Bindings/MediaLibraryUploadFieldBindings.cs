using Lombiq.Fields.Fields;
using Lombiq.Fields.Helpers;
using Orchard;
using Orchard.DynamicForms.Services;
using Orchard.DynamicForms.Services.Models;
using Orchard.Environment.Extensions;

namespace Lombiq.Fields.Bindings
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField.DynamicForms")]
    public class MediaLibraryUploadFieldBindings : Component, IBindingProvider
    {
        public void Describe(BindingDescribeContext context) =>
            context.For<MediaLibraryUploadField>().Binding("Ids", (contentItem, field, value) =>
                field.Ids = MediaLibraryUploadHelper.DecodeIds(value));
    }
}