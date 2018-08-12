using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectImageMetadata
    {
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public int? ImageId { get; set; }
        public string ImageName { get; set; }
        public string Caption { get; set; }
        public string Credit { get; set; }
        public string Keywords { get; set; }
        public bool IsWebsiteImage { get; set; }
        public bool IsWebsiteThumbnail { get; set; }
        public string NetworkPath { get; set; }
        public string CDRomPath { get; set; }
        public string ImageNumber { get; set; }
        public int? OrderNumber { get; set; }
        public string AccessLevel { get; set; }
        public string Comments { get; set; }
        public DateTime? UploadDate { get; set; }
        public string Device { get; set; }
        public float? latitude { get; set; }
        public float? longitude { get; set; }
        public string ImageHash { get; set; }
        public int? ImageSizeKB { get; set; }
        public string OriginalImageEndpoint { get; set; }
        public string WebsiteImageEndpoint { get; set; }
        public string ThumbnailImageEndpoint { get; set; }
    }
}