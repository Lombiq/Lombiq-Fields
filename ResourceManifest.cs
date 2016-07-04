using Orchard.UI.Resources;

namespace MediaKitty.Extensions
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineStyle("MediaLibraryUploadField").SetUrl("media-library-upload-field.min.css", "media-library-upload-field.css");

            manifest.DefineStyle("SlickTheme").SetUrl("../Content/Slick/slick/slick-theme.css");
            manifest.DefineStyle("Slick").SetUrl("../Content/Slick/slick/slick.css");
            manifest.DefineScript("Slick").SetUrl("../Content/Slick/slick/slick.min.js", "slick.js").SetDependencies("jQuery");
        }
    }
}