using Lombiq.Fields.Models;
using Orchard;

namespace Lombiq.Fields.Services
{
    /// <summary>
    /// Service for handling the files posted by <see cref="MediaLibraryUploadField"/> and the DynamicForms version of
    /// it <see cref="Lombiq.Fields.Elements.MediaLibraryUploadField"/>
    /// </summary>
    public interface IMediaLibraryUploadFieldPostHandlerService : IDependency
    {
        /// <summary>
        /// Validates and stores the files uploaded with either version of the field.
        /// </summary>
        /// <param name="context">The common context which is can be created by either version of the field.</param>
        void Handle(MediaLibraryUploadFieldPostHandlerContext context);
    }
}