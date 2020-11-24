/*
 * Copyright (c) 2012, IDM
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are permitted
 * provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright notice, this list of
 *       conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright notice, this list
 *       of conditions and the following disclaimer in the documentation and/or other materials
 *       provided with the distribution.
 *     * Neither the name of the IDM nor the names of its contributors may be used to endorse or
 *       promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
 * ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace IDM.SkPublish.API
{
    public class SkPublishAPI
    {
        /// <summary>
        /// Interface used to customize API requests.
        /// </summary>
        public interface RequestHandler
        {

            /// <summary>
            /// Preparation hook for HTTP GET requests.
            ///
            /// It allows setting custom headers such as "Accept: application/xml" to retrieve XML
            /// instead of JSON.
            /// </summary>
            /// <param name=request>the HTTP request</param>
            void PrepareGetRequest(HttpWebRequest request);
        }

        private string accessKey;

        private string baseUrl;

        private RequestHandler requestHandler;

        /// <summary>
        /// Creates a new and unconfigured API client.
        /// </summary>
        public SkPublishAPI()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Creates a new API client with preconfigured URL and access key.
        ///
        /// It will use the default HTTP client, which is known to be not thread-safe.
        /// </summary>
        /// <param name=baseUrl>the root URL of the SkPublish API.</param>
        /// <param name=accessKey>the access key to use.</param>
        public SkPublishAPI(string baseUrl, string accessKey)
            : this(baseUrl, accessKey, null)
        {
        }

        /// <summary>
        /// Creates a new API client with preconfigured URL, access key, API requests customization.
        ///
        /// It will use the default HTTP client, which is known to be not thread-safe.
        /// </summary>
        /// <param name=baseUrl>the root URL of the SkPublish API.</param>
        /// <param name=accessKey>the access key to use.</param>
        /// <param name=requestHandler>the object to use to customize the API requests.</param>
        public SkPublishAPI(string baseUrl, string accessKey, RequestHandler requestHandler)
        {
            this.baseUrl = baseUrl;
            this.accessKey = accessKey;
            this.requestHandler = requestHandler;
        }

        /// <summary>
        /// Make a spell checker search
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=searchWord>the searched word</param>
        /// <param name=entryNumber>the number of results</param>
        /// <returns>a list of suggestions.</returns>
        public string DidYouMean(string dictionaryCode, string searchWord, int? entryNumber)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);
            uri.Append("dictionaries/");
            uri.Append(dictionaryCode);
            uri.Append("/search/didyoumean?q=");
            uri.Append(HttpUtility.UrlEncode(searchWord, Encoding.UTF8));
            char c = '&';
            if (entryNumber != null) {
                uri.Append(c);
                uri.Append("entrynumber=");
                uri.Append(entryNumber.Value);
            }

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Gets the list of dictionaries.
        /// </summary>
        /// <returns>the list of dictionaries.</returns>
        public string GetDictionaries()
        {
            string res = ReadResponse(baseUrl + "dictionaries", true);
            return res;
        }

        /// <summary>
        /// Gets a dictionary.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <returns>the dictionary.</returns>
        public string GetDictionary(string dictionaryCode)
        {
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);

            string res = ReadResponse(baseUrl + "dictionaries/" + dictionaryCode, true);
            return res;
        }

        /// <summary>
        /// Gets an entry.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=entryId>the id of the entry.</param>
        /// <param name=format>the format of the entry, eitheir "html" or "xml".</param>
        /// <returns>the entry.</returns>
        public string GetEntry(string dictionaryCode, string entryId, string format)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);
            uri.Append("dictionaries/");
            uri.Append(dictionaryCode);
            uri.Append("/entries/");
            uri.Append(HttpUtility.UrlEncode(entryId, Encoding.UTF8));
            char c = '?';
            if (format != null) {
                if (!IsValidEntryFormat(format))
                    throw new ArgumentException(format);
                uri.Append(c);
                uri.Append("format=");
                uri.Append(format);
                c = '&';
            }

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Gets the list of pronunciations of an entry.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=entryId>the id of the entry.</param>
        /// <param name=lang>the lang of the pronunciation.</param>
        /// <returns>a list of pronunciations.</returns>
        public string GetEntryPronunciations(string dictionaryCode, string entryId, string lang)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);
            uri.Append("dictionaries/");
            uri.Append(dictionaryCode);
            uri.Append("/entries/");
            uri.Append(HttpUtility.UrlEncode(entryId, Encoding.UTF8));
            uri.Append("/pronunciations");
            char c = '?';
            if (lang != null) {
                if (!IsValidEntryLang(lang))
                    throw new ArgumentException(lang);
                uri.Append(c);
                uri.Append("lang=");
                uri.Append(lang);
                c = '&';
            }

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Gets the related entries of an entry.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=entryId>the id of the entry.</param>
        /// <param name=entryNumber>the number of results preceding/following the given entry</param>
        /// <returns>a list of entries.</returns>
        public string GetNearbyEntries(string dictionaryCode, string entryId, int? entryNumber)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);
            uri.Append("dictionaries/");
            uri.Append(dictionaryCode);
            uri.Append("/entries/");
            uri.Append(HttpUtility.UrlEncode(entryId, Encoding.UTF8));
            uri.Append("/nearbyentries");
            char c = '?';
            if (entryNumber != null) {
                uri.Append(c);
                uri.Append("entrynumber=");
                uri.Append(entryNumber.Value);
                c = '&';
            }

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Gets the related entries of an entry.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=entryId>the id of the entry.</param>
        /// <returns>a list of entries.</returns>
        public string GetRelatedEntries(string dictionaryCode, string entryId)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);
            uri.Append("dictionaries/");
            uri.Append(dictionaryCode);
            uri.Append("/entries/");
            uri.Append(HttpUtility.UrlEncode(entryId, Encoding.UTF8));
            uri.Append("/relatedentries");

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Gets a thesaurus list.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <returns>the thesaurus list.</returns>
        public string GetThesaurusList(string dictionaryCode)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);
            uri.Append("dictionaries/");
            uri.Append(dictionaryCode);
            uri.Append("/topics/");

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Gets a topic.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=thesaurusName>the name of the thesaurus.</param>
        /// <param name=topicId>the id of the topic.</param>
        /// <returns>the topic.</returns>
        public string GetTopic(string dictionaryCode, string thesName, string topicId)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);
            uri.Append("dictionaries/");
            uri.Append(dictionaryCode);
            uri.Append("/topics/");
            uri.Append(HttpUtility.UrlEncode(thesName, Encoding.UTF8));
            uri.Append("/");
            uri.Append(HttpUtility.UrlEncode(topicId, Encoding.UTF8));

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Gets a word of the day.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=day>the day.</param>
        /// <param name=format>the format of the entry.</param>
        /// <returns>a word of the day.</returns>
        public string GetWordOfTheDay(string dictionaryCode, string day, string format)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (dictionaryCode != null) {
                if (!IsValidDictionaryCode(dictionaryCode))
                    throw new ArgumentException(dictionaryCode);
                uri.Append("dictionaries/");
                uri.Append(dictionaryCode);
                uri.Append('/');
            }
            uri.Append("wordoftheday");
            char c = '?';
            if (day != null) {
                if (!IsValidWotdDay(day))
                    throw new ArgumentException(day);
                uri.Append(c);
                uri.Append("day=");
                uri.Append(day);
                c = '&';
            }
            if (format != null) {
                if (!IsValidEntryFormat(format))
                    throw new ArgumentException(format);
                uri.Append(c);
                uri.Append("format=");
                uri.Append(format);
                c = '&';
            }

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Gets a word of the day preview.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=day>the day.</param>
        /// <returns>the preview of a word of the day.</returns>
        public string GetWordOfTheDayPreview(string dictionaryCode, string day)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (dictionaryCode != null) {
                if (!IsValidDictionaryCode(dictionaryCode))
                    throw new ArgumentException(dictionaryCode);
                uri.Append("dictionaries/");
                uri.Append(dictionaryCode);
                uri.Append('/');
            }
            uri.Append("wordoftheday/preview");
            char c = '?';
            if (day != null) {
                if (!IsValidWotdDay(day))
                    throw new ArgumentException(day);
                uri.Append(c);
                uri.Append("day=");
                uri.Append(day);
                c = '&';
            }

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Make a search
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=searchWord>the searched word</param>
        /// <param name=pageSize>the number of results per page to return</param>
        /// <param name=pageIndex>the index of the result page to return</param>
        /// <returns>a list of entries.</returns>
        public string Search(string dictionaryCode, string searchWord, int? pageSize, int? pageIndex)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);
            uri.Append("dictionaries/");
            uri.Append(dictionaryCode);
            uri.Append("/search?q=");
            uri.Append(HttpUtility.UrlEncode(searchWord, Encoding.UTF8));
            char c = '&';
            if (pageSize != null) {
                uri.Append(c);
                uri.Append("pagesize=");
                uri.Append(pageSize.Value);
                c = '&';
            }
            if (pageIndex != null) {
                uri.Append(c);
                uri.Append("pageindex=");
                uri.Append(pageIndex.Value);
                c = '&';
            }

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        /// <summary>
        /// Gets the first search result.
        /// </summary>
        /// <param name=dictionaryCode>the code of the dictionary.</param>
        /// <param name=searchWord>the searched word</param>
        /// <param name=format>the format of the entry.</param>
        /// <returns>an entry.</returns>
        public string SearchFirst(string dictionaryCode, string searchWord, string format)
        {
            StringBuilder uri = new StringBuilder(baseUrl);
            if (!IsValidDictionaryCode(dictionaryCode))
                throw new ArgumentException(dictionaryCode);
            uri.Append("dictionaries/");
            uri.Append(dictionaryCode);
            uri.Append("/search/first?q=");
            uri.Append(HttpUtility.UrlEncode(searchWord, Encoding.UTF8));
            char c = '&';
            if (format != null) {
                if (!IsValidEntryFormat(format))
                    throw new ArgumentException(format);
                uri.Append(c);
                uri.Append("format=");
                uri.Append(format);
                c = '&';
            }

            string res = ReadResponse(uri.ToString(), true);
            return res;
        }

        private bool IsValidDictionaryCode(string code)
        {
            if (code.Length < 1)
                return false;
            for (int i = 0; i < code.Length; ++i)
            {
                char c = code[i];
                // Make sure no param are injected
                if (c == '/' || c == '%')
                    return false;
                if (c == '*' || c == '$')
                    return false;
            }
            return true;
        }

        private bool IsValidEntryFormat(string format)
        {
            for (int i = 0; i < format.Length; ++i)
            {
                char c = format[i];
                // Make sure no param are injected
                if (c == '/' || c == '%')
                    return false;
            }
            return true;
        }

        private bool IsValidEntryLang(string lang)
        {
            for (int i = 0; i < lang.Length; ++i)
            {
                char c = lang[i];
                // Make sure no param are injected
                if (c == '/' || c == '%')
                    return false;
            }
            return true;
        }

        private bool IsValidWotdDay(string day)
        {
            for (int i = 0; i < day.Length; ++i)
            {
                char c = day[i];
                // Make sure no param are injected
                if (c == '/' || c == '%')
                    return false;
            }
            return true;
        }

        private string ReadResponse(string uri, bool checkStatusCode)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("accessKey", accessKey);

            if (requestHandler != null)
                requestHandler.PrepareGetRequest(request);

            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
            }

            try
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string content = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();

                if (response.StatusCode == HttpStatusCode.OK)
                    return content;
                else
                    throw new SkPublishAPIException(response.StatusCode, content);
            }
            finally
            {
                response.Close();
            }
        }
    }
}
