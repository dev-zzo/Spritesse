using System.Collections.Generic;
using System.Xml;

namespace ThreeSheeps.Tiled
{
    /// <summary>
    /// Custom properties container.
    /// </summary>
    public sealed class TxxProperties : Dictionary<string, string>
    {
        // Empty for now

        public static TxxProperties FromParentXml(XmlNode root)
        {
            XmlNode properties = root.SelectSingleNode("properties");
            if (properties != null)
            {
                TxxProperties props = new TxxProperties();
                foreach (XmlNode child in root.ChildNodes)
                {
                    props.Add(child.Attributes["name"].Value, child.Attributes["value"].Value);
                }
                return props;
            }
            return null;
        }
    }
}
