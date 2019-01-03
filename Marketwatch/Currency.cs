using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Marketwatch {
    class Currency {
        public static CultureInfo GetCultureInfoByCurrencySymbol(string currencySymbol) {
            if (currencySymbol == null) {
                throw new ArgumentNullException("currencySymbol");
            }

            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Where(x => new RegionInfo(x.LCID).ISOCurrencySymbol == currencySymbol).First<CultureInfo>();
        }
    }
}
