using System;
using System.Collections.Generic;
using System.Text;

namespace Geocoding
{
	public class ParsedAddress : Address
	{
		public virtual string Street { get; set; }
		public virtual string City { get; set; }
		public virtual string County { get; set; }
		public virtual string State { get; set; }
		public virtual string Country { get; set; }
		public virtual string PostCode { get; set; }

		public ParsedAddress(string formattedAddress, Location coordinates, string provider)
			: base(formattedAddress, coordinates, provider) { }
	}
}
