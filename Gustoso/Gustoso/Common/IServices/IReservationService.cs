using Gustoso.Common.DTO.Communication;
using Gustoso.Common.DTO.Request;
using Gustoso.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.IServices
{
    public interface IReservationService
    {
        Task<Response<List<Reservation>>> GetReservationListAsync();
        Task<Response<List<Reservation>>> GetActiveReservationListAsync();
        Task<Response<string>> CreateReservationAsync(ReservationDTO dto);
        Task<Response<string>> ChangeStatusAsync(int id, Boolean isCanceled);
    }
}
