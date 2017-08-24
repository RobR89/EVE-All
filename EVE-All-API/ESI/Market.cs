using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

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
            lock (regionPages)
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
                // Constructor for JSON loading.
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

        public delegate void RegionHandler(int regionID);
        public static event RegionHandler RegionUpdate;

        /// <summary>
        /// Issue an event to all listeners telling them a region has been updated.
        /// </summary>
        /// <param name="regionID">The region ID of the region that was updated.</param>
        /// <remarks>the regionPages object must NOT be locked.</remarks>
        private static void IssueUpdate(int regionID)
        {
            bool entered = Monitor.IsEntered(regionPages);
            if (entered)
            {
                Monitor.Exit(regionPages);
            }
            RegionUpdate?.Invoke(regionID);
            if(entered)
            {
                Monitor.Enter(regionPages);
            }
        }

        public class MarketRegionPage : ESIList<MarketOrder>
        {
            public readonly int regionID;
            public MarketRegionPage(BinaryReader load)
            {
                regionID = load.ReadInt32();
                expire = new DateTime(load.ReadInt64());
                // Load the number of entries.
                int entries = load.ReadInt32();
                for (int i = 0; i < entries; i++)
                {
                    MarketOrder order = new MarketOrder(load);
                    items.Add(order);
                }
                lock (regionPages)
                {
                    regionPages[regionID] = this;
                }
                SetUpESI();
            }

            public MarketRegionPage(int region_ID)
            {
                regionID = region_ID;
                lock (regionPages)
                {
                    regionPages[regionID] = this;
                }
                SetUpESI();
            }

            private void SetUpESI()
            {
                // Set up ESI page.
                url = "markets/" + regionID + "/orders/";
                query = new Dictionary<string, string>
                {
                    ["order_type"] = "all"
                };
                autoUpdate = false;
                PageUpdated += MarketRegionPage_PageUpdated;
                ScheduleRefresh();
            }

            private void MarketRegionPage_PageUpdated(object page)
            {
                lock (regionPages)
                {
                    // Add regionID reference.
                    foreach (MarketOrder order in regionPages[regionID].items)
                    {
                        if (order == null)
                        {
                            continue;
                        }
                        order.regionID = regionID;
                    }
                }
                // Issue response outside of lock to prevent deadlock.
                // Issue update callback.
                IssueUpdate(regionID);
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

            public List<MarketOrder> GetOrdersForType(int typeID)
            {
                lock (this)
                {
                    return new List<MarketOrder>(items.Where(order => order.type_id == typeID));
                }
            }

            public List<MarketOrder> GetOrders()
            {
                lock (this)
                {
                    return new List<MarketOrder>(items);
                }
            }
        }

        private static Dictionary<int, MarketRegionPage> regionPages = new Dictionary<int, MarketRegionPage>();
        public static MarketRegionPage GetRegionPage(int regionID, bool create)
        {
            if(Universe.GetRegion(regionID) == null)
            {
                return null;
            }
            lock (regionPages)
            {
                if (!regionPages.ContainsKey(regionID))
                {
                    if(!create)
                    {
                        return null;
                    }
                    regionPages[regionID] = new MarketRegionPage(regionID);
                }
                return regionPages[regionID];
            }
        }

        public static List<MarketOrder> GetOrdersForType(int typeID)
        {
            List<MarketOrder> orders = new List<MarketOrder>();
            lock(regionPages)
            {
                foreach(MarketRegionPage page in regionPages.Values)
                {
                    orders.AddRange(page.GetOrdersForType(typeID));
                }
            }
            return orders;
        }

        public static List<MarketOrder> GetOrdersForRegion(int regionID)
        {
            MarketRegionPage page = GetRegionPage(regionID, false);
            if (page != null)
            {
                return page.GetOrders();
            }
            // Return empty list.
            return new List<MarketOrder>();
        }
        public static List<MarketOrder> GetOrdersForTypeAndRegion(int typeID, int regionID)
        {
            List<MarketOrder> type = GetOrdersForType(typeID);
            List<MarketOrder> region = GetOrdersForRegion(regionID);
            return new List<MarketOrder>(type.Intersect(region));
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

        public class PricesPage : ESIList<MarketValue>
        {
            public PricesPage()
            {
                url = "markets/prices/";
                autoUpdate = false;
                PageUpdated += PricesPage_PageUpdated;
                ScheduleRefresh();
            }

            private void PricesPage_PageUpdated(object page)
            {
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
        public static readonly PricesPage pricesPage = new PricesPage();

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

        #endregion

        #region MarketGroups
        public class MarketGroup : ESIPage
        {
            public MarketGroup(int groupID)
            {
                url = "markets/groups/" + groupID + "/";
                autoUpdate = false;
                ScheduleRefresh();
                lock(marketGroups)
                {
                    marketGroups[groupID] = this;
                }
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

        public class MarketGoupsPage : ESIList<int>
        {
            public MarketGoupsPage()
            {
                url = "markets/groups/";
                PageUpdated += MarketGoupsPage_PageUpdated;
                autoUpdate = false;
            }

            private void MarketGoupsPage_PageUpdated(object page)
            {
                List<int> groups = new List<int>();
                lock (this)
                {
                    if (items.Count > 0)
                    {
                        // Copy the list.
                        groups = new List<int>(items);
                    }
                }
                // Release the lock before creating more items that might cause deadlocks.
                foreach (int groupID in groups)
                {
                    if (!marketGroups.ContainsKey(groupID))
                    {
                        marketGroups[groupID] = new MarketGroup(groupID);
                    }
                    marketGroups[groupID].ScheduleRefresh();
                }
            }
        }

        public static readonly MarketGoupsPage marketGroupsPage = new MarketGoupsPage();
        #endregion

    }
}
