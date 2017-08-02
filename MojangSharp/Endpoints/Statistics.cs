﻿using MojangSharp.Api;
using MojangSharp.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MojangSharp.Endpoints
{
    public class Statistics : IEndpoint<StatisticsResponse>
    {

        /// <summary>
        /// Asks Mojang for its statistics about the given item
        /// </summary>
        public Statistics(List<Item> items) : this(items.ToArray()) { }

        /// <summary>
        /// Asks Mojang for its statistics about the given item
        /// </summary>
        public Statistics(params Item[] items)
        {
            this.Address = new Uri($"https://api.mojang.com/orders/statistics");
            foreach (Item item in items)
                this.Arguments.Add(Statistics.StatisticItems[item]);
        }

        public async override Task<StatisticsResponse> PerformRequest()
        {
            this.PostContent = "{ \"metricKeys\": [" + string.Join(",", this.Arguments.ConvertAll(x => $"\"{x.ToString()}\"").ToArray()) + "]}";
            this.Response = await Requester.Post(this);

            if (this.Response.IsSuccess)
            {
                JObject stats = JObject.Parse(this.Response.RawMessage);

                return new StatisticsResponse(this.Response)
                {
                    Total = stats["total"].ToObject<int>(),
                    Last24h = stats["last24h"].ToObject<int>(),
                    SaleVelocity = stats["saleVelocityPerSeconds"].ToObject<int>(),
                };
            }
            else
            {
                if (this.Response.Code == HttpStatusCode.BadRequest)
                {
                    return new StatisticsResponse(new Response(this.Response)
                    {
                        Error =
                        {
                            ErrorMessage = "One of the usernames is empty.",
                            ErrorTag = "IllegalArgumentException"
                        }
                    });
                }
                else
                    return new StatisticsResponse(Error.GetError(this.Response));
            }
        }

        /// <summary>
        /// List of available statistic items
        /// </summary>
        public static Dictionary<Item, string> StatisticItems = new Dictionary<Item, string>()
        {
            {  Item.MinecraftAccountsSold, "item_sold_minecraft" },
            {  Item.MinecraftPrepaidCardsRedeemed, "prepaid_card_redeemed_minecraft" },
            {  Item.CobaltAccountsSold, "item_sold_cobalt" },
            {  Item.ScrollsAccountsSold, "item_sold_scrolls" },
        };

        /// <summary>
        /// Available statistics entries
        /// </summary>
        public enum Item
        {
            /// <summary>
            /// Amount of minecraft accounts sold
            /// </summary>
            MinecraftAccountsSold,

            /// <summary>
            /// Amount of minecraft prepaid card redeemed
            /// </summary>
            MinecraftPrepaidCardsRedeemed,

            /// <summary>
            /// Amount of cobalt accounts sold
            /// </summary>
            CobaltAccountsSold,

            /// <summary>
            /// Amount of scrolls accounts sold
            /// </summary>
            ScrollsAccountsSold
        }
    }
    
}
