using Lombiq.Fields.Fields;
using Orchard;
using Orchard.DynamicForms.Services;
using Orchard.DynamicForms.Services.Models;
using System;
using System.Linq;

namespace Lombiq.Fields.Bindings
{
    public class MediaLibraryUploadFieldBindings : Component, IBindingProvider
    {
        public void Describe(BindingDescribeContext context)
        {
            context.For<MediaLibraryUploadField>()
                .Binding("MediaItem", (contentItem, field, s) =>
                {
                    field.Ids = String.IsNullOrEmpty(s) ? new int[0] : s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                });
        }
    }
}