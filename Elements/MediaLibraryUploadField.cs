using Orchard.DynamicForms.Elements;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Helpers;

namespace Lombiq.Fields.Elements
{
    [OrchardFeature("Lombiq.Fields.MediaLibraryUploadField.DynamicForms")]
    public class MediaLibraryUploadField : FormElement
    {
        /// <summary>
        /// The name of the hidden field which stores the already selected IDs. This is neccessary because we can't use
        /// the built one ProcessedVale because we want set it dynamically.
        /// </summary>
        public static string NameForSelectedIdsHiddenInput = "medialibraryuploadfield-selectedids-hiddeninput";


        public string DisplayName
        {
            get { return this.Retrieve(x => x.DisplayName); }
            set { this.Store(x => x.DisplayName, value); }
        }

        public string Hint
        {
            get { return this.Retrieve(x => x.Hint); }
            set { this.Store(x => x.Hint, value); }
        }

        public bool Required
        {
            get { return this.Retrieve(x => x.Required); }
            set { this.Store(x => x.Required, value); }
        }

        public bool Multiple
        {
            get { return this.Retrieve(x => x.Multiple); }
            set { this.Store(x => x.Multiple, value); }
        }

        public string AllowedExtensions
        {
            get { return this.Retrieve(x => x.AllowedExtensions); }
            set { this.Store(x => x.AllowedExtensions, value); }
        }

        public string FolderPath
        {
            get { return this.Retrieve(x => x.FolderPath); }
            set { this.Store(x => x.FolderPath, value); }
        }

        public int MaximumSizeKB
        {
            get { return this.Retrieve(x => x.MaximumSizeKB); }
            set { this.Store(x => x.MaximumSizeKB, value); }
        }

        public int FieldStorageUserQuotaMB
        {
            get { return this.Retrieve(x => x.FieldStorageUserQuotaMB); }
            set { this.Store(x => x.FieldStorageUserQuotaMB, value); }
        }

        public int ImageMaximumWidth
        {
            get { return this.Retrieve(x => x.ImageMaximumWidth); }
            set { this.Store(x => x.ImageMaximumWidth, value); }
        }

        public int ImageMaximumHeight
        {
            get { return this.Retrieve(x => x.ImageMaximumHeight); }
            set { this.Store(x => x.ImageMaximumHeight, value); }
        }

        public string MediaProfile
        {
            get { return this.Retrieve(x => x.MediaProfile); }
            set { this.Store(x => x.MediaProfile, value); }
        }

        public int[] Ids { get; set; }
    }
}