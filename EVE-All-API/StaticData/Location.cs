using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class Location
    {
        public readonly double x;
        public readonly double y;
        public readonly double z;

        private Location(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static Location ParseLocation(YamlNode node)
        {
            if (node.NodeType == YamlNodeType.Sequence)
            {
                YamlSequenceNode seq = (YamlSequenceNode)node;
                //string[] vals = seq.Value.ToString().Replace('[', ' ').Replace(']', ' ').Split(',');
                if (seq.Children.Count == 3)
                {
                    double _x = Double.Parse(seq.Children[0].ToString());
                    double _y = Double.Parse(seq.Children[1].ToString());
                    double _z = Double.Parse(seq.Children[2].ToString());
                    return new Location(_x, _y, _z);
                }
            }
            return null;
        }

    }
}
