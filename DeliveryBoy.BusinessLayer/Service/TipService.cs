using DeliveryBoy.BusinessLayer.Models;
using DeliveryBoy.BusinessLayer.Models.Dto;
using DeliveryBoy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryBoy.BusinessLayer.Service
{
    public interface ITipService
    {
        Response<bool> EnterTip(EnterTipModel model);
        Response<TipDto> GetTip(GetTipModel model);

        Response<TipDto> GetTip(double geoLat, double geoLong);
    }

    public class TipService : ITipService
    {
        private ITipsContext _Context;

        public TipService(ITipsContext context)
        {
            _Context = context;
        }

        public Response<bool> EnterTip(EnterTipModel model)
        {
            if (model.GeoLat == 0 || model.GeoLong == 0 || String.IsNullOrEmpty(model.Ip))
                return new Response<bool>(false);

            // See if they already entered one within a few feet in the last two days
            var mostRecentTip = _Context.Tips.Where(m => m.GeoLat == model.GeoLat && m.GeoLong == model.GeoLong).FirstOrDefault();

            if (mostRecentTip == null)
            {
                var utcNow = DateTime.UtcNow;

                var userTip = new UserTip()
                {
                    Ip = model.Ip,
                    CreatedOn = utcNow,
                    UpdatedOn = utcNow
                };

                var tip = new Tip()
                {
                    GeoLat = model.GeoLat,
                    GeoLong = model.GeoLong,
                    UpdatedOn = utcNow
                };

                tip = CalculateTip(tip, model);

                _Context.Tips.Add(tip);
                _Context.SaveChanges();

                userTip.TipId = tip.Id;
                _Context.UserTips.Add(userTip);
                _Context.SaveChanges();

                return new Response<bool>(true);
            }

            var mostRecent = _Context.UserTips.Where(m => m.Ip == model.Ip).FirstOrDefault();

            if (mostRecent.UpdatedOn > DateTime.UtcNow.AddDays(-2))
                return new Response<bool>(false);

            if(mostRecent == null)
            {
                var utcNow = DateTime.UtcNow;

                var userTip = new UserTip()
                {
                    Ip = model.Ip,
                    CreatedOn = utcNow,
                    UpdatedOn = utcNow
                };

                var tip = new Tip()
                {
                    GeoLat = model.GeoLat,
                    GeoLong = model.GeoLong,
                    UpdatedOn = utcNow
                };

                tip = CalculateTip(tip, model);

                _Context.Tips.Add(tip);
                _Context.SaveChanges();

                userTip.TipId = tip.Id;
                _Context.UserTips.Add(userTip);
                _Context.SaveChanges();
            }
            else
            {
                var utcNow = DateTime.UtcNow;
                mostRecent.UpdatedOn = utcNow;

                mostRecentTip = CalculateTip(mostRecentTip, model);

                _Context.Tips.Update(mostRecentTip);
                _Context.SaveChanges();

            }

            return new Response<bool>(true);
        }

        public Response<TipDto> GetTip(double geoLat, double geoLong)
        {
            double meters = 100; // Going to round to nearest 50 meters

            // number of km per degree = ~111km (111.32 in google maps, but range varies
            //between 110.567km at the equator and 111.699km at the poles)
            // 1km in degree = 1 / 111.32km = 0.0089
            // 1m in degree = 0.0089 / 1000 = 0.0000089
            double coef = meters * 0.0000089;

            double max_lat = geoLat + coef;
            double max_long = geoLong + coef / Math.Cos(geoLat * 0.018);

            double min_lat = geoLat - coef;
            double min_long = geoLong - coef / Math.Cos(geoLat * 0.018);

            var allTips = _Context.Tips.Where(m => m.GeoLat <= max_lat && m.GeoLong <= min_lat && m.GeoLat >= min_lat && m.GeoLong >= min_lat).ToList();

            var tipDto = new TipDto();
            tipDto.NoTipPercentage = allTips.Sum(x => x.NoTip);
            tipDto.LowTipPercentage = allTips.Sum(x => x.LowTip);
            tipDto.GoodTipPercentage = allTips.Sum(x => x.GoodTip);
            tipDto.GreatTipPercentage = allTips.Sum(x => x.GreatTip);

            return new Response<TipDto>(tipDto);
        }

        public Response<TipDto> GetTip(GetTipModel model)
        {

            return null;
        }

        private Tip CalculateTip(Tip tip, EnterTipModel model)
        {
            if (model.NoTip)
                tip.NoTip++;
            if (model.LowTip)
                tip.LowTip++;
            if (model.GoodTip)
                tip.GoodTip++;
            if (model.GreatTip)
                tip.GreatTip++;

            return tip;
        }
    }
}
