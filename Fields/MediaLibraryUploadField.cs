using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage;
using Orchard.ContentManagement.Utilities;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Models;

namespace Lombiq.Fields.Fields
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField")]
    public class MediaLibraryUploadField : ContentField
    {
        private static readonly char[] _separators = new[] { '{', '}', ',' };

        private readonly LazyField<IEnumerable<MediaPart>> _mediaParts = new LazyField<IEnumerable<MediaPart>>();
        internal LazyField<IEnumerable<MediaPart>> MediaPartsField { get { return _mediaParts; } }
        public IEnumerable<MediaPart> MediaParts { get { return _mediaParts.Value; } }

        public int[] Ids
        {
            get { return DecodeIds(Storage.Get<string>()); }
            set { Storage.Set(EncodeIds(value)); }
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


        private string EncodeIds(ICollection<int> ids)
        {
            if (ids == null || !ids.Any()) return string.Empty;

            // Using {1},{2} format so it can be filtered with delimiters.
            return "{" + string.Join("},{", ids.ToArray()) + "}";
        }

        private int[] DecodeIds(string ids)
        {
            if (String.IsNullOrWhiteSpace(ids)) return new int[0];

            return ids.Split(_separators, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }
    }
}