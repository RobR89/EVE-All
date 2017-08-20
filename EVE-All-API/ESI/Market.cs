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
#region caching
        public static void SaveAll(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(dir);
            FileStream file = File.Open(fileName, FileMode.Create);
            BinaryWriter save = new BinaryWriter(file);
            // Save the data.
            SaveMarketValues(save);
            SaveMarketOrders(save);
        }

        public static void LoadAll(string fileName)
        {
            if(!File.Exists(fileName))
            {
                return;
            }
            FileStream file = File.Open(fileName, FileMode.Open);
            BinaryReader load = new BinaryReader(file);
            // Load the data.
            LoadMarketValues(load);
            LoadMarketOrders(load);
        }

        public static void SaveMarketOrders(BinaryWriter save)
        {
            lock (regionPages)
            {
                // Save the expire time.
                save.Write(regionPages.Count);
                foreach (MarketRegionPage region in regionPages.Values)
                {
                    region.Save(save);
                }
            }
        }

        public static void LoadMarketOrders(BinaryReader load)
        {
            lock (marketOrders)
            {
                // Load the expire time.
                int entries = load.ReadInt32();
                for (int i = 0; i < entries; i++)
                {
                    MarketRegionPage page = new MarketRegionPage(load);
                }
            }
        }

        public static void SaveMarketValues(BinaryWriter save)
        {
            lock (marketValues)
            {
                // Save the expire time.
                save.Write(pricesPage.expire.Ticks);
                // Save the number of entries.
                save.Write(marketValues.Count);
                foreach (MarketValue value in marketValues.Values)
                {
                    value.Save(save);
                }
            }
        }

        public static void LoadMarketValues(BinaryReader load)
        {
            lock (marketValues)
            {
                // Load the expire time.
                pricesPage.expire = new DateTime(load.ReadInt64());
                // Load the number of entries.
                int entries = load.ReadInt32();
                for (int i = 0; i < entries; i++)
                {
                    MarketValue value = new MarketValue(load);
                    marketValues[value.type_id] = value;
                }
            }
        }

        #endregion caching

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

            public void Save(BinaryWriter save)
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

            public bool IsOrderExpired()
            {
                double age = (DateTime.Now - issued).TotalDays;
                return age > duration;
            }
        }

        private class MarketRegionPage : ESIList<MarketOrder>
        {
            public int regionID;
            public MarketRegionPage(BinaryReader load)
            {
                regionID = load.ReadInt32();
                expire = new DateTime(load.ReadInt64());
                SetUpESI();
                // Load the number of entries.
                int entries = load.ReadInt32();
                for (int i = 0; i < entries; i++)
                {
                    MarketOrder order = new MarketOrder(load);
                    items.Add(order);
                    AddOrder(order);
                }
            }

            public MarketRegionPage(int region_ID)
            {
                regionID = region_ID;
                SetUpESI();
            }
            private void SetUpESI()
            {
                // Set up ESI page.
                url = "markets/" + regionID + "/orders/";
                autoUpdate = false;
                query = new Dictionary<string, string>
                {
                    ["order_type"] = "all"
                };
            }

            public void Save(BinaryWriter save)
            {
                save.Write(regionID);
                save.Write(expire.Ticks);
                // Save the number of entries.
                save.Write(items.Count);
                foreach (MarketOrder order in items)
                {
                    order.Save(save);
                }
            }

        }
        private static Dictionary<int, MarketRegionPage> regionPages = new Dictionary<int, MarketRegionPage>();

        public static void UpdateRegionMarket(int regionID)
        {
            lock (regionPages)
            {
                if (!regionPages.ContainsKey(regionID))
                {
                    regionPages[regionID] = new MarketRegionPage(regionID);
                }
                JSON.JSONResponse resp = regionPages[regionID].GetPage();
                if (resp.httpCode == System.Net.HttpStatusCode.OK)
                {
                    lock (marketOrders)
                    {
                        foreach (MarketOrder order in regionPages[regionID].items)
                        {
                            order.regionID = regionID;
                            AddOrder(order);
                        }
                    }
                }
            }
        }

        private static Dictionary<long, MarketOrder> marketOrders = new Dictionary<long, MarketOrder>();
        public static MarketOrder GetOrder(long orderID)
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
        public static List<long> GetOrdersForType(int typeID)
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
        public static List<long> GetOrdersForRegion(int regionID)
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
        public static List<long> GetOrdersForTypeAndRegion(int typeID, int regionID)
        {
            List<long> type = GetOrdersForType(typeID);
            List<long> region = GetOrdersForRegion(regionID);
            return new List<long>(type.Intersect(region));
        }

        /// <summary>
        /// Add an order and indexs.
        /// </summary>
        /// <param name="order">The order to add</param>
        /// <remarks>Must be called with marketOrders locked.</remarks>
        private static void AddOrder(MarketOrder order)
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
            public void Save(BinaryWriter save)
            {
                save.Write(type_id);
                save.Write(adjusted_price);
                save.Write(average_price);
            }
        }

        private class PricesPage : ESIList<MarketValue>
        {
            public PricesPage()
            {
                url = "markets/prices/";
                autoUpdate = false;
            }
        }
        private static PricesPage pricesPage = new PricesPage();

        private static Dictionary<int, MarketValue> marketValues = new Dictionary<int, MarketValue>();
        public static MarketValue GetMarketValue(int typeID)
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

        public static void UpdateMarketValues()
        {
            lock (pricesPage)
            {
                JSON.JSONResponse resp = pricesPage.GetPage();
                if (pricesPage.items.Count == 0)
                {
                    // No data returned.
                    return;
                }
                lock (marketValues)
                {
                    // Update expire time.
                    marketValues.Clear();
                    foreach (MarketValue value in pricesPage.items)
                    {
                        marketValues[value.type_id] = value;
                    }
                }
            }
        }

        #endregion

        #region MarketGroups
        public class MarketGroup : ESIPage
        {
            public MarketGroup(int groupID)
            {
                url = "markets/groups/" + groupID + "/";
                autoUpdate = false;
                autoUpdateAction = new Action<Task>(_ => Refresh()); ;
            }
            protected void Refresh()
            {
                GetPage();
            }
            public int market_group_id;
            public string name;
            public string description;
            public List<int> types;
            public int parent_group_id;
        }

        private static Dictionary<int, MarketGroup> marketGroups = new Dictionary<int, MarketGroup>();
        public static MarketGroup GetMarketGroup(int groupID)
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

        private static ESIList<int> groupIDs = new ESIList<int>()
        {
            url = "markets/groups/"
        };
        public static void UpdateMarketGroups()
        {
            groupIDs.GetPage();
            if (groupIDs.items.Count > 0)
            {
                lock (marketGroups)
                {
                    foreach (int groupID in groupIDs.items)
                    {
                        if (!marketGroups.ContainsKey(groupID))
                        {
                            marketGroups[groupID] = new MarketGroup(groupID);
                        }
                        marketGroups[groupID].ScheduleRefresh();
                    }
                }
            }
        }
        #endregion

    }
}
