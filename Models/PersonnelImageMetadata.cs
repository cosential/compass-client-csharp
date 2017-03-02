using System;

namespace Cosential.Integrations.Compass.Client
{
    public class PersonnelImageMetadata
    {
        public long PersonnelId { get; set; }
        public long ImageId { get; set; }
        public string ImageName { get; set; }
        public string Caption { get; set; }
        public string Credit { get; set; }
        public string Keywords { get; set; }
        public string NetworkPath { get; set; }
        public string CDRomPath { get; set; }
        public int? OrderNumber { get; set; }
        public string Comments { get; set; }
        public DateTime? UploadDate { get; set; }
        public string Device { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string ImageHash { get; set; }
        public int? ImageSizeKB { get; set; }
        public string OriginalImageEndpoint { get; set; }
        public string WebsiteImageEndpoint { get; set; }
        public string ThumbnailImageEndpoint { get; set; }
    }
}