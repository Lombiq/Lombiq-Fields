using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.Fields.Models
{
    public interface IMediaLibraryUploadSettings
    {
        string AllowedExtensions { get; }
        int MaximumSizeKB { get; }
        int ImageMaximumWidth { get; }
        int ImageMaximumHeight { get; }
        bool Multiple { get; }
        int FieldStorageUserQuotaMB { get; }
        bool Required { get; }
        string Hint { get; }
    }
}