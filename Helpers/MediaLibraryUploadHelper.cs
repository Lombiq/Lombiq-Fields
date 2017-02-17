using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.Fields.Helpers
{
    public static class MediaLibraryUploadHelper
    {
        private static readonly char[] _separators = new[] { '{', '}', ',' };


        public static string EncodeIds(ICollection<int> ids)
        {
            if (ids == null || !ids.Any()) return string.Empty;

            // Using {1},{2} format so it can be filtered with delimiters.
            return "{" + string.Join("},{", ids.ToArray()) + "}";
        }

        public static int[] DecodeIds(string ids)
        {
            if (String.IsNullOrWhiteSpace(ids)) return new int[0];

            return ids.Split(_separators, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }
    }
}