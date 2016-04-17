using System;
using System.Xml;

namespace ThreeSheeps.Tiled
{
    internal static class XmlNodeExtensions
    {
        public static string GetStringAttribute(this XmlNode node, string name, string defaultValue = null)
        {
            XmlAttribute attr = node.Attributes[name];
            return attr == null ? defaultValue : attr.Value;
        }

        public static bool ParseBoolAttribute(this XmlNode node, string name, bool defaultValue = false)
        {
            XmlAttribute attr = node.Attributes[name];
            if (attr != null)
            {
                string value = attr.Value.ToLower();
                if (value == "1" || value == "on" || value == "true")
                    return true;
                if (value == "0" || value == "off" || value == "false")
                    return false;
                throw new FormatException("invalid value for a bool");
            }
            return defaultValue;
        }

        public static int ParseIntAttribute(this XmlNode node, string name, int defaultValue = 0)
        {
            XmlAttribute attr = node.Attributes[name];
            if (attr != null)
            {
                return int.Parse(attr.Value);
            }
            return defaultValue;
        }

        public static uint ParseUIntAttribute(this XmlNode node, string name, uint defaultValue = 0)
        {
            XmlAttribute attr = node.Attributes[name];
            if (attr != null)
            {
                return uint.Parse(attr.Value);
            }
            return defaultValue;
        }

        public static float ParseFloatAttribute(this XmlNode node, string name, float defaultValue = 0.0f)
        {
            XmlAttribute attr = node.Attributes[name];
            if (attr != null)
            {
                return float.Parse(attr.Value);
            }
            return defaultValue;
        }
    }
}
