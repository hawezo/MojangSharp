﻿using MojangSharp.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojangSharp.Responses
{

    /// <summary>
    /// Response to an authenticate request
    /// </summary>
    public class AuthenticateResponse : Response
    {
        internal AuthenticateResponse(Response response) : base(response) { }

        /// <summary>
        /// Access Token for this user
        /// </summary>
        public string AccessToken { get; internal set; }

        /// <summary>
        /// Must be the same as Requester.ClientToken
        /// </summary>
        public string ClientToken { get; internal set; }

        /// <summary>
        /// List of available profiles
        /// </summary>
        public List<Uuid> AvailableProfiles { get; internal set; }

        /// <summary>
        /// Last profile selected by the user
        /// </summary>
        public Uuid SelectedProfile { get; internal set; }

        /// <summary>
        /// User data sent by requestUser
        /// </summary>
        public UserData User { get; internal set; }

        /// <summary>
        /// Represents data sent by requestUser option
        /// </summary>
        public class UserData
        {
            /// <summary>
            /// User UUID
            /// </summary>
            [JsonProperty("id")]
            public string Uuid { get; internal set; }

            /// <summary>
            /// Properties of this user
            /// </summary>
            [JsonProperty("properties")]
            public List<Property> Properties { get; internal set; }

            /// <summary>
            /// Represents a user property
            /// </summary>
            public class Property
            {
                [JsonProperty("name")]
                public string Name { get; internal set; }

                [JsonProperty("value")]
                public string Value { get; internal set; }
            }

        }

    }

    /// <summary>
    /// Represents a response to a payload who returns token
    /// </summary>
    public class TokenResponse : Response
    {
        internal TokenResponse(Response response) : base(response) { }

        /// <summary>
        /// Access token for this instance
        /// </summary>
        public string AccessToken { get; internal set; }

    }

    // ---

    /// <summary>
    /// Represents an error occured while using authentication API
    /// </summary>
    public class AuthenticationResponseError : Error
    {
        internal AuthenticationResponseError(JObject json)
        {
            this.ErrorTag = json["error"].ToObject<string>();
            this.ErrorMessage = json["errorMessage"].ToObject<string>();
            if (json.ToString().Contains("cause"))
                this.Cause = json["cause"].ToObject<string>();
        }

        public string Cause { get; private set; }
    }

}
