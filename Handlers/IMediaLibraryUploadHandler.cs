using Lombiq.Fields.Fields;
using Lombiq.Fields.Models;
using Orchard.Events;

namespace Lombiq.Fields.Handlers
{
    /// <summary>
    /// Events for the files posted by <see cref="MediaLibraryUploadField"/> and the DynamicForms version of
    /// it (<see cref="Lombiq.Fields.Elements.MediaLibraryUploadField"/>).
    /// </summary>
    public interface IMediaLibraryUploadHandler : IEventHandler
    {
        /// <summary>
        /// Validates and stores the files uploaded with either version of the field.
        /// </summary>
        /// <param name="context">The common context which can be created by either version of the field.</param>
        void ValidateAndStore(MediaLibraryUploadingContext context);
    }
}