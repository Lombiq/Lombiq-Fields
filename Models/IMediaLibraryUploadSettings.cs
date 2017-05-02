using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.Fields.Models
{
    /// <summary>
    /// The common settings between the field and the element.
    /// </summary>
    public interface IMediaLibraryUploadSettings
    {
        /// <summary>
        /// A comma-separated list of the file extensions that are allowed to be uploaded. Leaving it empty will make the field to accept all file types.
        /// </summary>
        string AllowedExtensions { get; }

        /// <summary>
        /// The maximum file size per uploaded file in kilobytes. 0 means no limit.
        /// </summary>
        int MaximumSizeKB { get; }

        /// <summary>
        /// The maximum width for uploaded images in pixels. 0 means no limit.
        /// </summary>
        int ImageMaximumWidth { get; }

        /// <summary>
        /// The maximum height for uploaded images in pixels. 0 means no limit.
        /// </summary>
        int ImageMaximumHeight { get; }

        /// <summary>
        /// Whether allowed to the user to upload multiple items.
        /// </summary>
        bool Multiple { get; }

        /// <summary>
        /// The maximum size of all uploaded files for this field in megabytes. 0 means no limit.
        /// </summary>
        int FieldStorageUserQuotaMB { get; }

        /// <summary>
        /// Whether uploading at least one item is required.
        /// </summary>
        bool Required { get; }

        /// <summary>
        /// The hint text of the field.
        /// </summary>
        string Hint { get; }
    }
}