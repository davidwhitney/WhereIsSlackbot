﻿using System;
using System.Web;
using WhereIs.Commands;

namespace WhereIs.Infrastructure
{
    public class UrlHelper : IUrlHelper
    {
        private readonly Configuration _config;

        public UrlHelper(Configuration config)
        {
            _config = config ?? throw new ArgumentException("Expected an instance of Configuration to be injected by the runtime.", nameof(config));
        }

        public string ImageFor(string locationKey) => $"{ForUrl(nameof(MapCommand.Map))}&key={HttpUtility.UrlEncode(locationKey)}";

        private string ForUrl(string functionName)
        {
            var apiKey = _config.ApiKey;
            var apiRoot = _config.UrlRoot;
            return $"{apiRoot}/{functionName}?code={apiKey}";
        }
    }
}