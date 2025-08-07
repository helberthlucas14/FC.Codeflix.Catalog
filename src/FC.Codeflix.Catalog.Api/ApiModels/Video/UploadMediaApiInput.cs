using FC.Codeflix.Catalog.Api.Extensions;
using FC.Codeflix.Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.Api.ApiModels.Video;

public class UploadMediaApiInput
{
    private static class MediaType
    {
        public const string Banner = "banner";
        public const string Thumb = "thumbnail";
        public const string ThumbHalf = "thumbnail_half";
        public const string Media = "video";
        public const string Trailer = "trailer";
    }

    [FromForm(Name = "media_file")]
    public IFormFile Media { get; set; }
}
