using System.Collections.Generic;
using System.Xml;

namespace EVE_All_API
{
    public class xmlUtils
    {
        public static XmlAttribute newAttribute(XmlNode root, string name, string text)
        {
            XmlAttribute nAttrib = root.OwnerDocument.CreateAttribute(name);
            nAttrib.Value = text;
            root.Attributes.Append(nAttrib);
            return nAttrib;
        }

        public static XmlElement newElement(XmlNode root, string name, string text)
        {
            XmlElement nElement = root.OwnerDocument.CreateElement(name);
            nElement.InnerText = text;
            root.AppendChild(nElement);
            return nElement;
        }

        /// <summary>
        /// Check that the dictionary has all the columns.
        /// </summary>
        /// <param name="columns">The column names to search for.</param>
        /// <param name="row">The row to check.</param>
        /// <returns>True if all columns are present.</returns>
        public static bool checkColumns(string[] columns, Dictionary<string, string> row)
        {
            foreach (string column in columns)
            {
                if (!row.ContainsKey(column))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Parse a rowset element into a list of dictonary rows and check that all columns exist.
        /// </summary>
        /// <param name="node">The node to parse.</param>
        /// <param name="name">The rowset name or null to ignore.</param>
        /// <param name="rows">The list of rows found.</param>
        /// <param name="columns">The columns to check for.</param>
        /// <returns>Ture if successful.</returns>
        public static bool parseRowSet(XmlNode node, string name, out List<Dictionary<string, string>> rows, string[] columns)
        {
            bool good = parseRowSet(node, name, out rows);
            if(rows == null)
            {
                return good;
            }
            foreach(Dictionary<string, string> row in rows)
            {
                if (!checkColumns(columns, row))
                {
                    return false;
                }
            }
            return good;
        }

        /// <summary>
        /// Parse a rowset element into a list of dictionary rows.
        /// </summary>
        /// <param name="node">The node to parse.</param>
        /// <param name="name">The rowset name or null to ignore.</param>
        /// <param name="rows">The list of rows found.</param>
        /// <returns>Ture if successful.</returns>
        public static bool parseRowSet(XmlNode node, string name, out List<Dictionary<string, string>> rows)
        {
            if (node.Name != "rowset")
            {
                rows = null;
                return false;
            }
            if (name != null)
            {
                XmlNode nameAttr = node.Attributes.GetNamedItem("name");
                if (nameAttr == null || nameAttr.Value != name)
                {
                    rows = null;
                    return false;
                }
            }
            rows = new List<Dictionary<string, string>>();
            // Parse each row.
            foreach (XmlNode rowsetNode in node.ChildNodes)
            {
                if (rowsetNode.Name == "row")
                {
                    // Create a dictionary for the row.
                    Dictionary<string, string> row = new Dictionary<string, string>();
                    foreach (XmlNode rowNode in rowsetNode.Attributes)
                    {
                        // Add the value to the row.
                        row[rowNode.Name] = rowNode.Value;
                    }
                    if (row.Count > 0)
                    {
                        // There were values save the row.
                        rows.Add(row);
                    }
                }
            }
            return true;
        }

    }
}
