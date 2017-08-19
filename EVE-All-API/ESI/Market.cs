using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVE_All_API.ESI
{
    public class Market
    {
        public static void saveAll(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(dir);
            FileStream file = File.Open(fileName, FileMode.Create);
            BinaryWriter save = new BinaryWriter(file);
            // Save the data.
            saveMarketValues(save);
            saveMarketOrders(save);
        }

        public static void loadAll(string fileName)
        {
            if(!File.Exists(fileName))
            {
                return;
            }
            FileStream file = File.Open(fileName, FileMode.Open);
            BinaryReader load = new BinaryReader(file);
            // Load the data.
            loadMarketValues(load);
            loadMarketOrders(load);
        }

        #region MarketOrder
        public class MarketOrder
        {
            public long order_id;
            public int type_id;
            public long location_id;
            public int volume_total;
            public int volume_remain;
            public int min_volume;
            public double price;
            public bool is_buy_order;
            public int duration;
            public DateTime issued;
            public string range;
            // Non-JSON values.
            public int regionID;

            public MarketOrder()
            {
            }
            public MarketOrder(BinaryReader load)
            {
                order_id = load.ReadInt64();
                type_id = load.ReadInt32();
                location_id = load.ReadInt64();
                volume_total = load.ReadInt32();
                volume_remain = load.ReadInt32();
                min_volume = load.ReadInt32();
                price = load.ReadDouble();
                is_buy_order = load.ReadBoolean();
                duration = load.ReadInt32();
                issued = new DateTime(load.ReadInt64());
                range = load.ReadString();
                regionID = load.ReadInt32();
            }

            public void save(BinaryWriter save)
            {
                save.Write(order_id);
                save.Write(type_id);
                save.Write(location_id);
                save.Write(volume_total);
                save.Write(volume_remain);
                save.Write(min_volume);
                save.Write(price);
                save.Write(is_buy_order);
                save.Write(duration);
                save.Write(issued.Ticks);
                save.Write(range);
                save.Write(regionID);
            }

            public bool isExpired()
            {
                double age = (DateTime.Now - issued).TotalDays;
                return age > duration;
            }
        }

        private static Dictionary<long, MarketOrder> marketOrders = new Dictionary<long, MarketOrder>();
        private static Dictionary<int, DateTime> marketRegionExpire = new Dictionary<int, DateTime>();
        public static MarketOrder getOrder(long orderID)
        {
            lock(marketOrders)
            {
                if(marketOrders.ContainsKey(orderID))
                {
                    return marketOrders[orderID];
                }
            }
            return null;
        }
        private static Dictionary<int, List<long>> typeOrders = new Dictionary<int, List<long>>();
        public static List<long> getOrdersForType(int typeID)
        {
            List<long> result = new List<long>();
            lock (marketOrders)
            {
                if(typeOrders.ContainsKey(typeID))
                {
                    result.AddRange(typeOrders[typeID]);
                }
            }
            return result;
        }
        private static Dictionary<int, List<long>> regionOrders = new Dictionary<int, List<long>>();
        public static List<long> getOrdersForRegion(int regionID)
        {
            List<long> result = new List<long>();
            lock (marketOrders)
            {
                if (regionOrders.ContainsKey(regionID))
                {
                    result.AddRange(regionOrders[regionID]);
                }
            }
            return result;
        }
        public static List<long> getOrdersForTypeAndRegion(int typeID, int regionID)
        {
            List<long> type = getOrdersForType(typeID);
            List<long> region = getOrdersForRegion(regionID);
            return new List<long>(type.Intersect(region));
        }

        public static void saveMarketOrders(BinaryWriter save)
        {
            lock (marketOrders)
            {
                // Save the expire time.
                save.Write(marketRegionExpire.Count);
                foreach (var region in marketRegionExpire)
                {
                    save.Write(region.Key);
                    save.Write(region.Value.Ticks);
                }
                // Save the number of entries.
                save.Write(marketOrders.Count);
                foreach (MarketOrder order in marketOrders.Values)
                {
                    order.save(save);
                }
            }
        }

        public static void loadMarketOrders(BinaryReader load)
        {
            lock (marketOrders)
            {
                // Load the expire time.
                int entries = load.ReadInt32();
                for (int i = 0; i < entries; i++)
                {
                    int regionID = load.ReadInt32();
                    marketRegionExpire[regionID] = new DateTime(load.ReadInt64());
                }
                // Load the number of entries.
                entries = load.ReadInt32();
                for (int i = 0; i < entries; i++)
                {
                    MarketOrder order = new MarketOrder(load);
                    addOrder(order);
                }
            }
        }

        /// <summary>
        /// Add an order and indexs.
        /// </summary>
        /// <param name="order">The order to add</param>
        /// <remarks>Must be called with marketOrders locked.</remarks>
        private static void addOrder(MarketOrder order)
        {
            marketOrders[order.order_id] = order;
            if (!typeOrders.ContainsKey(order.type_id))
            {
                typeOrders[order.type_id] = new List<long>();
            }
            typeOrders[order.type_id].Add(order.order_id);
            if (!regionOrders.ContainsKey(order.regionID))
            {
                regionOrders[order.regionID] = new List<long>();
            }
            regionOrders[order.regionID].Add(order.order_id);
        }

        public static void updateRegionMarket(int regionID)
        {
            // Check expire time.
            lock(marketRegionExpire)
            {
                if(marketRegionExpire.ContainsKey(regionID))
                {
                    if(marketRegionExpire[regionID] > DateTime.Now)
                    {
                        return;
                    }
                }
            }
            // Data expired, load new.
            string url = "markets/" + regionID + "/orders/";
            string query = "&order_type=all";
            JSON.ESIList<MarketOrder> market = JSON.getESIlist<MarketOrder>(url, query);
            if (market.items.Count == 0)
            {
                // No data returned.
                return;
            }
            lock (marketRegionExpire)
            {
                // Update expire time.
                marketRegionExpire[regionID] = market.expires;
            }
            lock(marketOrders)
            {
                foreach (MarketOrder order in market.items)
                {
                    order.regionID = regionID;
                    addOrder(order);
                }
            }
        }

        #endregion

        #region MarketValues
        public class MarketValue
        {
            public int type_id;
            public double average_price;
            public double adjusted_price;

            public MarketValue()
            {
            }
            public MarketValue(BinaryReader load)
            {
                type_id = load.ReadInt32();
                adjusted_price = load.ReadDouble();
                average_price = load.ReadDouble();
            }
            public void save(BinaryWriter save)
            {
                save.Write(type_id);
                save.Write(adjusted_price);
                save.Write(average_price);
            }
        }

        private static Dictionary<int, MarketValue> marketValues = new Dictionary<int, MarketValue>();
        private static DateTime marketValuesExpires = DateTime.Now;

        public static MarketValue getMarketValue(int typeID)
        {
            lock (marketValues)
            {
                if (marketValues.ContainsKey(typeID))
                {
                    return marketValues[typeID];
                }
            }
            return null;
        }

        public static void saveMarketValues(BinaryWriter save)
        {
            lock (marketValues)
            {
                // Save the expire time.
                save.Write(marketValuesExpires.Ticks);
                // Save the number of entries.
                save.Write(marketValues.Count);
                foreach (MarketValue value in marketValues.Values)
                {
                    value.save(save);
                }
            }
        }

        public static void loadMarketValues(BinaryReader load)
        {
            lock (marketValues)
            {
                // Load the expire time.
                marketValuesExpires = new DateTime(load.ReadInt64());
                // Load the number of entries.
                int entries = load.ReadInt32();
                for (int i = 0; i < entries; i++)
                {
                    MarketValue value = new MarketValue(load);
                    marketValues[value.type_id] = value;
                }
            }
        }

        public static void updateMarketValues()
        {
            // Check expire time.
            if (marketValuesExpires > DateTime.Now)
            {
                return;
            }
            // Data expired, load new.
            JSON.ESIList<MarketValue> values = JSON.getESIlist<MarketValue>("markets/prices/");
            if(values.items.Count == 0)
            {
                // No data returned.
                return;
            }
            lock(marketValues)
            {
                // Update expire time.
                marketValuesExpires = values.expires;
                marketValues.Clear();
                foreach(MarketValue value in values.items)
                {
                    marketValues[value.type_id] = value;
                }
            }
        }

        #endregion

        #region MarketGroups
        public class MarketGroup
        {
            public int market_group_id;
            public string name;
            public string description;
            public List<int> types;
            public int parent_group_id;
        }

        private static Dictionary<int, MarketGroup> marketGroups = new Dictionary<int, MarketGroup>();
        public static MarketGroup getMarketGroup(int groupID)
        {
            lock(marketGroups)
            {
                if(marketGroups.ContainsKey(groupID))
                {
                    return marketGroups[groupID];
                }
            }
            return null;
        }

        public static void updateMarketGroups()
        {
            JSON.ESIList<int> groupIDs = JSON.getESIlist<int>("markets/groups/");
            if (groupIDs.items.Count > 0)
            {
                List<MarketGroup> groups = new List<MarketGroup>();
                foreach (int groupID in groupIDs.items)
                {
                    JSON.ESIItem<MarketGroup> group = JSON.getESIItem<MarketGroup>("markets/groups/" + groupID + "/");
                    if(group != null)
                    {
                        groups.Add(group.item);
                    }
                }
                Dictionary<int, MarketGroup> newGroups = new Dictionary<int, MarketGroup>();
                foreach (MarketGroup group in groups)
                {
                    newGroups[group.market_group_id] = group;
                }
                lock (marketGroups)
                {
                    marketGroups = newGroups;
                }
            }
        }
        #endregion

    }
}
