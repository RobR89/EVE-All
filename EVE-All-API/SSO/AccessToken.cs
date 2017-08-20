using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace EVE_All_API
{
    public class AccessToken
    {
        private static List<AccessToken> accessTokens = new List<AccessToken>();
        public static List<AccessToken> GetAccessTokens()
        {
            lock (accessTokens)
            {
                return new List<AccessToken>(accessTokens);
            }
        }
        public static void AddToken(AccessToken token)
        {
            if (token == null)
            {
                return;
            }
            lock (accessTokens)
            {
                accessTokens.Add(token);
            }
            token.GetCharacterInfo();
            AccessTokenAdded?.Invoke(token);
        }

        public static List<AccessToken> GetTokensForCharacter(long characterID)
        {
            List<AccessToken> tokens = new List<AccessToken>();
            lock (accessTokens)
            {
                foreach (AccessToken token in accessTokens)
                {
                    if (token?.CharacterID == characterID)
                    {
                        tokens.Add(token);
                    }
                }
            }
            return tokens;
        }

        public static AccessToken GetFirstTokenForCharacter(long characterID)
        {
            lock (accessTokens)
            {
                foreach (AccessToken token in accessTokens)
                {
                    if (token?.CharacterID == characterID)
                    {
                        return token;
                    }
                }
            }
            return null;
        }

        public static AccessToken GetTokenForCharacterWithScope(long characterID, string scope)
        {
            List<AccessToken> tokens = GetTokensForCharacter(characterID);
            foreach(AccessToken token in tokens)
            {
                if(token?.HasScope(scope) == true)
                {
                    return token;
                }
            }
            return null;
        }

        public static AccessToken AddToken(string access_token, string token_type, string refresh_token, long expires_in)
        {
            AccessToken token = new AccessToken()
            {
                access_token = access_token,
                token_type = token_type,
                refresh_token = refresh_token,
                expires_in = expires_in,
                generated = DateTime.Now
            };
            AddToken(token);
            return token;
        }
        public delegate void AccessTokenHandler(AccessToken token);
        public static event AccessTokenHandler AccessTokenAdded;

        // Token data.
        public string access_token;
        public string token_type;
        public string refresh_token;
        public long expires_in;
        public DateTime generated;

        /// <summary>
        /// Is the token expired?
        /// </summary>
        /// <returns>true if token expired.</returns>
        public bool IsExpired()
        {
            TimeSpan span = DateTime.Now - generated;
            return (span.TotalSeconds >= expires_in);
        }

        /// <summary>
        /// Refresh the token.
        /// </summary>
        /// <returns>true if successfuly refreshed.</returns>
        public bool Refresh()
        {
            if (!String.IsNullOrEmpty(refresh_token))
            {
                return FetchToken(refresh_token);
            }
            return false;
        }

        /// <summary>
        /// Character information.
        /// </summary>
        public long CharacterID = 0;
        public string CharacterName = null;
        public DateTime ExpiresOn;
        public string Scopes = null;
        public string TokenType = null;
        public string CharacterOwnerHash = null;

        /// <summary>
        /// Check to see if the token has the desited scope.
        /// </summary>
        /// <param name="scope">The scope to find.</param>
        /// <returns>True if the token has the scope.</returns>
        public bool HasScope(string scope)
        {
            string[] scopes = Scopes.Split(' ');
            return Array.IndexOf(scopes, scope) > -1;
        }

        /// <summary>
        /// Get the character info from the auth server.
        /// </summary>
        public void GetCharacterInfo()
        {
            string url = "https://login.eveonline.com/oauth/verify";
            JSON.JSONResponse resp = JSON.GetJSONPage(url, null, this);
            if (resp?.httpCode == HttpStatusCode.OK)
            {
                JsonConvert.PopulateObject(resp.content, this);
                Pilot pilot = Pilot.GetPilot(CharacterID);
                pilot.characterSheet.name = CharacterName;
            }
        }

        /// <summary>
        /// Fetch the access token from the SSO server.
        /// </summary>
        /// <param name="token">The token to populate.</param>
        /// <param name="code">The access code to use, will be a refresh if it matches the refresh token.</param>
        /// <returns>true if successful</returns>
        public bool FetchToken(string code)
        {
            // Get the URL for the token.
            string tokenURI = "https://login.eveonline.com/oauth/token";
            // Get the autherization code for the server.
            string auth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(UserData.sso_ClientID + ":" + UserData.sso_SecurityKey));
            // Get the token values.
            string tokenValues = "";
            if (refresh_token == code)
            {
                tokenValues = "grant_type=refresh_token&refresh_token=" + code;
            }
            else
            {
                tokenValues = "grant_type=authorization_code&code=" + code;
            }
            byte[] data = Encoding.ASCII.GetBytes(tokenValues);

            // Construct the request to CCP.
            Uri uri = new Uri(tokenURI);
            HttpWebRequest tokenRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            tokenRequest.Method = WebRequestMethods.Http.Post;
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            // Add the authorization header.
            tokenRequest.Headers.Add("Authorization", "Basic " + auth);
            // Add the grant and code data.
            tokenRequest.ContentLength = data.Length;
            using (var stream = tokenRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            // Get the response form CCP.
            HttpWebResponse tokenResponse = null;
            try
            {
                tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            }
            catch (WebException e)
            {
                tokenResponse = (HttpWebResponse)e.Response;
            }
            if (tokenResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(tokenResponse.GetResponseStream());
                string content = reader.ReadToEnd();
                JsonConvert.PopulateObject(content, this);
                generated = DateTime.Now;
                // Auto save the new token as refresh tokens can only be used once and we don't want to loose the new token.
                UserData.SaveConfig();
                return true;
            }
            else
            {
                StreamReader reader = new StreamReader(tokenResponse.GetResponseStream());
                string content = reader.ReadToEnd();
                System.Diagnostics.Debug.WriteLine("SSO Error: " + content);
                return false;
            }
        }

    }
}
