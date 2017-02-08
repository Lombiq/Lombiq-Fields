using Orchard.UI.Resources;

namespace Lombiq.Fields
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineStyle("MediaLibraryUploadField").SetUrl("media-library-upload-field.min.css", "media-library-upload-field.css");
        }
    }
}