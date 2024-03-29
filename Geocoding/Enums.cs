﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Geocoding
{
	public enum ResponseStatus : int
	{
		Ok = 0,
		OkBatch = 100,
		ErrorInput = 400,
		ErrorAccountKey = 403,
		ErrorUnknown = 500,
	}
	public enum DataFormat
	{
		json,
		xml,
		csv,
	}

	public enum LocationType
	{
		/// <summary>
		/// Stop: default
		/// </summary>
		s,
		/// <summary>
		/// Via
		/// </summary>
		v,
	}

	public enum Quality : int
	{
		/// <summary>
		/// P1	A specific point location.
		/// </summary>
		POINT = 0,
		/// <summary>
		/// L1	A specific street address location.
		/// </summary>
		ADDRESS = 1,
		/// <summary>
		/// I1	An intersection of two or more streets.
		/// </summary>
		INTERSECTION = 2,
		/// <summary>
		/// B1	The center of a single street block. House number ranges are returned if available.
		/// B2	The center of a single street block, which is located closest to the geographic center of all matching street blocks. No house number range is returned.
		/// B3	The center of a single street block whose numbered range is nearest to the input number. House number range is returned.
		/// </summary>
		STREET = 3,
		/// <summary>
		/// Z2	Postal code. For USA, a ZIP+2.
		/// Z3	Postal code. For USA, a ZIP+4.
		/// </summary>
		ZIP_EXTENDED = 4,
		/// <summary>
		/// Z1	Postal code, largest. For USA, a ZIP.
		/// Z4	Postal code, smallest. Unused in USA.
		/// </summary>
		ZIP = 5,
		/// <summary>
		/// A6 Admin area. For USA, a neighborhood.
		/// </summary>
		NEIGHBORHOOD = 6,
		/// <summary>
		/// A5	Admin area. For USA, a city.
		/// </summary>
		CITY = 7,
		/// <summary>
		/// A4	Admin area. For USA, a county.
		/// </summary>
		COUNTY = 8,
		/// <summary>
		/// A3	Admin area. For USA, a state.
		/// </summary>
		STATE = 9,
		/// <summary>
		/// A1	Admin area, largest. For USA, a country.
		/// </summary>
		COUNTRY = 10,

		UNKNOWN = 11
	}

	public enum SideOfStreet
	{
		/// <summary>
		/// None: default
		/// </summary>
		N,
		/// <summary>
		/// Left
		/// </summary>
		L,
		/// <summary>
		/// Right
		/// </summary>
		R,
		/// <summary>
		/// Mixed
		/// </summary>
		M,
	}
}
