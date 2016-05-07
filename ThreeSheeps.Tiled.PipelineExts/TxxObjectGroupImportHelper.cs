using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;

namespace ThreeSheeps.Tiled
{
    static class TxxObjectGroupImportHelper
    {
        public static TxxObjectGroupContent Import(XmlNode root)
        {
            TxxObjectGroupContent group = new TxxObjectGroupContent();

            group.Name = root.GetStringAttribute("name");
            group.Opacity = root.ParseFloatAttribute("opacity", 1.0f);
            group.Visible = root.ParseBoolAttribute("visible", true);
            group.RenderingOffset.X = root.ParseIntAttribute("offsetx");
            group.RenderingOffset.Y = root.ParseIntAttribute("offsety");
            group.Properties = TxxProperties.FromParentXml(root);

            foreach (XmlNode node in root.SelectNodes("object"))
            {
                group.Objects.Add(ImportObject(node));
            }

            return group;
        }

        private static TxxObjectContent ImportObject(XmlNode root)
        {
            TxxObjectContent content;

            XmlNode polygonNode = root["polygon"];
            XmlNode polylineNode = root["polyline"];
            if (polygonNode != null || polylineNode != null)
            {
                TxxPolyObjectContent theContent;
                string points;
                if (polygonNode != null)
                {
                    theContent = new TxxPolygonObjectContent();
                    points = polygonNode.GetStringAttribute("points", "");
                }
                else
                {
                    theContent = new TxxPolylineObjectContent();
                    points = polylineNode.GetStringAttribute("points", "");
                }
                content = theContent;
                List<Point> pointsArray = new List<Point>();
                foreach (string pointData in points.Split(" ".ToCharArray()))
                {
                    string[] pointCoords = pointData.Split(",".ToCharArray());
                    pointsArray.Add(new Point(int.Parse(pointCoords[0]), int.Parse(pointCoords[1])));
                }
                theContent.Points = pointsArray.ToArray();
            }
            else
            {
                TxxBoundedObjectContent theContent;
                if (root["ellipse"] != null)
                {
                    theContent = new TxxEllipseObjectContent();
                }
                else
                {
                    theContent = new TxxRectangleObjectContent();
                }
                content = theContent;
                theContent.Dimensions.X = root.ParseIntAttribute("width");
                theContent.Dimensions.Y = root.ParseIntAttribute("height");
            }

            content.Name = root.GetStringAttribute("name");
            content.Type = root.GetStringAttribute("type");
            content.Id = root.ParseIntAttribute("id");
            content.Gid = root.ParseIntAttribute("gid");
            content.Position.X = root.ParseIntAttribute("x");
            content.Position.Y = root.ParseIntAttribute("y");
            content.Rotation = root.ParseFloatAttribute("rotation");
            content.Visible = root.ParseBoolAttribute("visible", true);
            content.Properties = TxxProperties.FromParentXml(root);

            return content;
        }
    }
}
