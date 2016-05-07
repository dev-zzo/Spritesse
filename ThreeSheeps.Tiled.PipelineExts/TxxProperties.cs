using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;

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
                    XmlNode nameNode = child.Attributes["name"];
                    if (nameNode == null || String.IsNullOrWhiteSpace(nameNode.Value))
                    {
                        throw new InvalidContentException("found a property without a name");
                    }
                    props.Add(nameNode.Value, child.GetStringAttribute("value"));
                }
                return props;
            }
            return null;
        }
    }
}
