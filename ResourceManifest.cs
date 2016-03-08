using Orchard.UI.Resources;

namespace MediaKitty.Extensions
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineStyle("MediaLibraryUploadField").SetUrl("media-library-upload-field.min.css", "media-library-upload-field.css");

            manifest.DefineStyle("SlickTheme").SetUrl("slick-theme.css");
            manifest.DefineStyle("Slick").SetUrl("slick.css");
            manifest.DefineScript("Slick").SetUrl("slick.min.js", "slick.js");
        }
    }
}