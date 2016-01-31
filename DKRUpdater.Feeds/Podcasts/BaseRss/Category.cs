using System.Xml.Serialization;

namespace DKRUpdater.Feeds.Podcasts.BaseRss
{
    [XmlRoot(ElementName = "category", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public class Category
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }
}
