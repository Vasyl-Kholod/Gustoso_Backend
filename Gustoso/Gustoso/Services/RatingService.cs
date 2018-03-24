using Gustoso.Common.DTO.Communication;
using Gustoso.Common.DTO.Request;
using Gustoso.Common.IServices;
using Gustoso.Common.Models;
using Gustoso.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Services
{
    public class RatingService: IRatingService
    {
        private readonly MSContext _db;

        public RatingService(MSContext context)
        {
            _db = context;
        }

        public async Task<Response<List<Rating>>> GetAllRatingAsync(string userName)
        {
            var response = new Response<List<Rating>>();
            var ratings = await _db.Ratings.Where(r => r.userName == userName).ToListAsync();
            if(ratings.ToArray().Length > 0)
            {
                response.Data = ratings;
            } else
            {
                response.Error = new Error(404, "Ratings not found");
                return response;
            }
            return response;
        }

        public async Task<Response<string>> SetRatingAsync(string userName, RatingDTO ratingDTO)
        {
            var response = new Response<string>();
            var ratingObjDB = await _db.Ratings
                .AsNoTracking()
                .Where(r => r.userName == userName && r.slideName == ratingDTO.slideName)
                .FirstOrDefaultAsync();
            if(ratingObjDB == null)
            {
                var ratingObj = new Rating()
                {
                    userName = userName,
                    slideName = ratingDTO.slideName,
                    ratingValue = ratingDTO.ratingValue
                };
                await _db.Ratings.AddAsync(ratingObj);
            } else
            {
                ratingObjDB.ratingValue = ratingDTO.ratingValue;
                _db.Ratings.Update(ratingObjDB);
            }

            await _db.SaveChangesAsync();
            response.Data = "Rating is success saved!";
            return response;
        }
    }
}
