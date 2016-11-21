using System;
using System.Collections.Generic;
using System.Linq;
using HobknobClientNet.Api;
using HobknobClientNet.Api.Response;

namespace HobknobClientNet
{
    internal class FeatureToggleProvider
    {
        private readonly ApiClient _apiClient;

        public FeatureToggleProvider(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IEnumerable<KeyValuePair<string, bool>> Get()
        {
            var features = new List<KeyValuePair<string, bool>>();

            var apiResponse = _apiClient.Get();

            if (apiResponse?.Categories == null)
            {
                return features;
            }

            BuildSimpleFeatureToggles(features, apiResponse);

            BuildMultiFeatureToggles(features, apiResponse);

            return features;
        }

        private void BuildMultiFeatureToggles(List<KeyValuePair<string, bool>> features, ApiResponse apiResponse)
        {
            var multiToggleCategories = apiResponse.Categories.Where(x => x.Value.Id != 0);

            foreach (var category in multiToggleCategories)
            {
                foreach (var feature in category.Value.Features)
                {
                    var valueIndex = 0;

                    foreach (var value in feature.Values)
                    {
                        if (value.HasValue)
                        {
                            features.Add(new KeyValuePair<string, bool>(feature.Name + "/" + category.Value.Columns[valueIndex], value.Value));
                        }

                        valueIndex++;
                    }
                }
            }
        }

        private void BuildSimpleFeatureToggles(List<KeyValuePair<string, bool>> features, ApiResponse apiResponse)
        {
            var simpleToggles = apiResponse.Categories.FirstOrDefault(x => x.Value.Id == 0);

            if (!simpleToggles.Value.Features.Any())
            {
                return;
            }

            features.AddRange(simpleToggles.Value.Features.Select(feature => new KeyValuePair<string, bool>(feature.Name, (bool)feature.Values.First())));
        }
    }
}
