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
        public static List<AccessToken> getAccessTokens()
        {
            lock (accessTokens)
            {
                return new List<AccessToken>(accessTokens);
            }
        }
        public static void addToken(AccessToken token)
        {
            if (token == null)
            {
                return;
            }
            lock (accessTokens)
            {
                accessTokens.Add(token);
            }
            token.getCharacterInfo();
            AccessTokenAdded?.Invoke(token);
        }

        public static List<AccessToken> getTokensForCharacter(long characterID)
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

        public static AccessToken getFirstTokenForCharacter(long characterID)
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

        public static AccessToken getTokenForCharacterWithScope(long characterID, string scope)
        {
            List<AccessToken> tokens = getTokensForCharacter(characterID);
            foreach(AccessToken token in tokens)
            {
                if(token?.hasScope(scope) == true)
                {
                    return token;
                }
            }
            return null;
        }

        public static AccessToken addToken(string access_token, string token_type, string refresh_token, long expires_in)
        {
            AccessToken token = new AccessToken();
            token.access_token = access_token;
            token.token_type = token_type;
            token.refresh_token = refresh_token;
            token.expires_in = expires_in;
            token.generated = DateTime.Now;
            addToken(token);
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
        public bool isExpired()
        {
            TimeSpan span = DateTime.Now - generated;
            return (span.TotalSeconds >= expires_in);
        }

        /// <summary>
        /// Refresh the token.
        /// </summary>
        /// <returns>true if successfuly refreshed.</returns>
        public bool refresh()
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
        public bool hasScope(string scope)
        {
            string[] scopes = Scopes.Split(' ');
            return Array.IndexOf(scopes, scope) > -1;
        }

        /// <summary>
        /// Get the character info from the auth server.
        /// </summary>
        public void getCharacterInfo()
        {
            string url = "https://login.eveonline.com/oauth/verify";
            JSON.ESIResponse resp = JSON.getESIPage(url, null, this);
            if (resp?.code == HttpStatusCode.OK)
            {
                Pilot pilot = Pilot.getPilot(CharacterID);
                JsonConvert.PopulateObject(resp.content, this);
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
