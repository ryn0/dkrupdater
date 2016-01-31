using DKRUpdater.Feeds.Podcasts.Interfaces;
using System;
using System.Xml.Serialization;

namespace DKRUpdater.Feeds.Podcasts.BaseRss
{
    [XmlRoot(ElementName = "rss")]
    public class RssRootBase : IRss
    {
        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "content", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Content { get; set; }
        [XmlAttribute(AttributeName = "wfw", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Wfw { get; set; }
        [XmlAttribute(AttributeName = "dc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Dc { get; set; }
        [XmlAttribute(AttributeName = "atom", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Atom { get; set; }
        [XmlAttribute(AttributeName = "sy", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Sy { get; set; }
        [XmlAttribute(AttributeName = "slash", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Slash { get; set; }
        [XmlAttribute(AttributeName = "itunes", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Itunes { get; set; }
        [XmlAttribute(AttributeName = "rawvoice", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Rawvoice { get; set; }
        [XmlAttribute(AttributeName = "googleplay", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Googleplay { get; set; }

        public string Media
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }

}
