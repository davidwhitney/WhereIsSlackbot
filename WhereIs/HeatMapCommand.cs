﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WhereIs.CapacityMonitoring;
using WhereIs.FindingPlaces;
using WhereIs.ImageGeneration;
using WhereIs.Slack;

namespace WhereIs
{
    public class HeatMapCommand
    {
        private readonly LocationCollection _locations;
        private readonly IMemoryCache _cache;
        private readonly ICapacityService _capacityService;
        private readonly IImageGenerator _generator;

        public HeatMapCommand(LocationCollection locations, IImageGenerator generator, IMemoryCache cache,
            ICapacityService capacityService)
        {
            _locations = locations ?? throw new ArgumentNullException(nameof(locations));
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _capacityService = capacityService;
        }

        [FunctionName("HeatMap")]
        public IActionResult Execute([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                var mapKey = req.Query["key"].FirstOrDefault()?.ToLower();

                var pointsOfInterest = _locations.Where(x => x.RawKey().StartsWith(mapKey + "::")).ToList();

                var hotness = new Hotness();

                var highlights = new List<Highlight>();
                foreach (var poi in pointsOfInterest)
                {
                    var location = _locations.Single(x=>x.Key == poi.Key);
                    var totalAvailableSeats = location.Capacity;
                    var filledSeats = _capacityService.NumberOfDesksOccupiedForLocation(poi.RawKey());
                    filledSeats = filledSeats > totalAvailableSeats ? totalAvailableSeats : filledSeats;

                    var percentage = (int)Math.Floor((double)filledSeats / (double)totalAvailableSeats * 100);
                    
                    var colorGrade = hotness.Rank(percentage);

                    highlights.Add(new Highlight(poi.ImageLocation, colorGrade));
                }

                var outputBytes = _generator.HighlightMap(mapKey, highlights);

                return new FileContentResult(outputBytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }
    }
}