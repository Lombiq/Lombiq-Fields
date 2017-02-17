using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage;
using Orchard.ContentManagement.Utilities;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Models;
using Lombiq.Fields.Helpers;

namespace Lombiq.Fields.Fields
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadField : ContentField
    {
        private readonly LazyField<IEnumerable<MediaPart>> _mediaParts = new LazyField<IEnumerable<MediaPart>>();
        internal LazyField<IEnumerable<MediaPart>> MediaPartsField { get { return _mediaParts; } }
        public IEnumerable<MediaPart> MediaParts { get { return _mediaParts.Value; } }

        public int[] Ids
        {
            get { return MediaLibraryUploadHelper.DecodeIds(Storage.Get<string>()); }
            set { Storage.Set(MediaLibraryUploadHelper.EncodeIds(value)); }
        }

        public string FirstMediaUrl { get { return MediaParts.Any() ? MediaParts.First().MediaUrl : string.Empty; } }

        public string MediaProfile
        {
            get
            {
                return PartFieldDefinition.Settings.ContainsKey("MediaLibraryUploadFieldSettings.MediaProfile") ?
                    PartFieldDefinition.Settings["MediaLibraryUploadFieldSettings.MediaProfile"] : string.Empty;
            }
        }
    }
}