﻿using AmbientSounds.Models;
using Microsoft.Toolkit.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AmbientSounds.Services
{
    /// <summary>
    /// Retrieves sound data from an online source.
    /// </summary>
    public class OnlineSoundDataProvider : IOnlineSoundDataProvider
    {
        private readonly ISystemInfoProvider _systemInfoProvider;
        private readonly HttpClient _client;
        private const string _url = ""; // do not commit

        public OnlineSoundDataProvider(
            HttpClient httpClient,
            ISystemInfoProvider systemInfoProvider)
        {
            Guard.IsNotNull(systemInfoProvider, nameof(systemInfoProvider));
            Guard.IsNotNull(httpClient, nameof(httpClient));
            _systemInfoProvider = systemInfoProvider;
            _client = httpClient;
        }

        /// <inheritdoc/>
        public async Task<IList<Sound>> GetSoundsAsync()
        {
            if (string.IsNullOrWhiteSpace(_url))
            {
                return new List<Sound>();
            }

            var url = _url + $"?culture={_systemInfoProvider.GetCulture()}";
            using Stream result = await _client.GetStreamAsync(url);
            return (await JsonSerializer.DeserializeAsync<Sound[]>(result)) ?? new Sound[0];
        }

        /// <inheritdoc/>
        public async Task<IList<Sound>> GetSoundsAsync(IList<string> soundIds)
        {
            if (soundIds == null || soundIds.Count == 0)
            {
                return new Sound[0];
            }

            var sounds = await GetSoundsAsync();
            return sounds?.Where(x => x.Id != null && soundIds.Contains(x.Id)).ToArray()
                ?? new Sound[0];
        }
    }
}
