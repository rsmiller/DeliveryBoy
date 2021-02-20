using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geocoding
{
	public class MapQuestResult
	{
		[JsonProperty("locations")]
		public IList<MapQuestLocation> Locations { get; set; }

		[JsonProperty("providedLocation")]
		public MapQuestLocation ProvidedLocation { get; set; }
	}
}
