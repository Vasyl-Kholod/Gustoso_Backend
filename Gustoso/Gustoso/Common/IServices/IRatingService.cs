using Gustoso.Common.DTO.Communication;
using Gustoso.Common.DTO.Request;
using Gustoso.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.IServices
{
    public interface IRatingService
    {
        Task<Response<List<Rating>>> GetAllRatingAsync(string userName);

        Task<Response<string>> SetRatingAsync(string userName, RatingDTO ratingDTO);
    }
}
