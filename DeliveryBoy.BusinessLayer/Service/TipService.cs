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
        private const double _ScanRadius = 0.003;

        private ITipsContext _Context;

        public TipService(ITipsContext context)
        {
            _Context = context;
        }

        public Response<bool> EnterTip(EnterTipModel model)
        {
            if (model.GeoLat == 0 || model.GeoLong == 0 || String.IsNullOrEmpty(model.Ip))
                return new Response<bool>(false);

            // Entering in detail
            var utcNow = DateTime.UtcNow;

            var tipDetail = new TipDetail()
            {
                GeoLat = model.GeoLat,
                GeoLong = model.GeoLong,
                NoTip = model.NoTip,
                LowTip = model.LowTip,
                GoodTip = model.Good,
                GreatTip = model.GreatTip,
                CreatedOn = utcNow
            };

            _Context.TipDetails.Add(tipDetail);
            _Context.SaveChanges();


            // See if they already entered one within a few feet in the last two days
            var mostRecentTip = _Context.Tips.Where(m => m.GeoLat == model.GeoLat && m.GeoLong == model.GeoLong).FirstOrDefault();

            if (mostRecentTip == null)
            {
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
                mostRecent.UpdatedOn = utcNow;

                mostRecentTip = CalculateTip(mostRecentTip, model);

                _Context.Tips.Update(mostRecentTip);
                _Context.SaveChanges();

            }

            return new Response<bool>(true);
        }

        public Response<TipDto> GetTip(double geoLat, double geoLong)
        {
            double max_lat = geoLat + _ScanRadius;
            double max_long = geoLong + _ScanRadius;

            double min_lat = geoLat - _ScanRadius;
            double min_long = geoLong - _ScanRadius;

            var allTips = _Context.Tips.Where(m => 
                    m.GeoLat <= max_lat && 
                    m.GeoLong <= min_lat && 
                    m.GeoLat >= min_lat && 
                    m.GeoLong >= min_lat).ToList();

            var tipDto = new TipDto();
            var noTip = allTips.Sum(x => x.NoTip);
            var lowTip = allTips.Sum(x => x.LowTip);
            var goodTip = allTips.Sum(x => x.GoodTip);
            var greatTip = allTips.Sum(x => x.GreatTip);
            var total = noTip + lowTip + goodTip + greatTip;

            tipDto.NoTipPercentage = noTip / total;
            tipDto.LowTipPercentage = lowTip / total;
            tipDto.GoodTipPercentage = goodTip / total;
            tipDto.GreatTipPercentage = greatTip / total;

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
