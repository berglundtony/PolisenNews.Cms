using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;
using System.Xml;

namespace PolisenNews.Cms.Models.Medias
{
    [ContentType(DisplayName = "Vector Image File",
        GUID = "3bedeaa0-67ba-4f6a-a420-dabf6ad6890b",
        Description = "Used for svg image file type")]
    [MediaDescriptor(ExtensionString = "svg")]
    public class VectorImageMediaData : ImageMediaData
    {
        /// <summary>
        /// Gets the generated thumbnail for this media.
        /// </summary>
        public override Blob Thumbnail { get => BinaryData; }

        public virtual string XML
        {
            get
            {
                try
                {
                    var blob = BinaryData;
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(blob.OpenRead());
                    return xmlDoc.InnerXml;
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}
