using Gustoso.Common.DTO.Communication;
using Gustoso.Common.DTO.Request;
using Gustoso.Common.IServices;
using Gustoso.Common.Models;
using Gustoso.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Gustoso.Services
{
    public class ReservationService: IReservationService
    {
        private readonly MSContext _db;
        private readonly string _email;
        private readonly string _password;
        private readonly string _host;
        private static object _smtpLocker = new object();
        private static readonly HttpClient client = new HttpClient();

        public ReservationService(IConfigurationRoot root, MSContext context)
        {
            _db = context;
            var config = root.GetSection("SmtpConfig");
            _email = config["email"];
            _password = config["password"];
            _host = config["host"];
        }

        public async Task<Response<List<Reservation>>> GetReservationListAsync()
        {
            var response = new Response<List<Reservation>>();
            var todayDateNow = DateTime.Now;
            var data = await _db.Reservations.Where(r => r.isConfirmed == false && r.dateOfReservation >= todayDateNow).ToListAsync();
            if(data.Count.ToString() == "0")
            {
                response.Error = new Error(404, "Reservation not found");
            } else
            {
                response.Data = data;
            }
            return response;
        }

        public async Task<Response<List<Reservation>>> GetActiveReservationListAsync()
        {
            var response = new Response<List<Reservation>>();
            var todayDateNow = DateTime.Now;
            var todayDateMax = new DateTime(todayDateNow.Year, todayDateNow.Month, todayDateNow.Day, 23, 59, 59, 999);
            var reservation = await _db.Reservations.Where(r => r.isConfirmed == true && (r.dateOfReservation >= todayDateNow && r.dateOfReservation <= todayDateMax)).ToListAsync();
            var reservationStatus = await _db.ReservationStatus.Where(s => s.status == "confirm").ToArrayAsync();
            foreach(var el in reservation)
            {
                var data = reservationStatus.Where(s => s.id_reservation == el.id).FirstOrDefault() as ReservationStatus;
                if(data == null)
                {
                    reservation.Remove(el);
                }
            }
            if (reservation.Count.ToString() == "0")
            {
                response.Error = new Error(404, "Reservation not found");
            }
            else
            {
                response.Data = reservation;
            }
            return response;
        }

        public async Task<Response<string>> CreateReservationAsync(ReservationDTO dto)
        {
            var response = new Response<string>();
            var dateOfReservation = new DateTime(dto.Year, dto.Month, dto.Day, dto.Hour, dto.Minute, 0);
            var objToBD = new Reservation()
            {
                clientName = dto.clientName,
                clientPhone = dto.clientPhone,
                clientEmail = dto.clientEmail,
                tableNumber = dto.tableNumber,
                dateOfReservation = dateOfReservation,
                isConfirmed = false
            };
            var result = await _db.Reservations.AddAsync(objToBD);
            if(result.State.ToString() == "Added")
            {
                await _db.SaveChangesAsync();
                response.Data = "Reservation save succesed!";
            } else
            {
                response.Error = new Error(500, "Error adding reservation!");
            }
            
            return response;
        }

        public async Task<Response<string>> ChangeStatusAsync(int id, Boolean isCaceled = true)
        {
            var response = new Response<string>();
            var reservation = await _db.Reservations.AsNoTracking().Where(r => r.id == id).FirstOrDefaultAsync();
            reservation.isConfirmed = true;
            _db.Reservations.Update(reservation);
            var reservationStatus = new ReservationStatus
            {
                id_reservation = reservation.id,
                status = isCaceled ? "reject" : "confirm"
            };
            await _db.ReservationStatus.AddAsync(reservationStatus);
            await _db.SaveChangesAsync();
            await sendToEmail(reservation, isCaceled);
            response.Data = "Status has been successfully changed";
            return response;
        }

        public async Task sendToEmail(Reservation obj, Boolean status)
        {

            string emailBody = "";
            string file = status ? "rejectReservationEmailBody.txt" : "confirmReservationEmailBody.txt";
            using (StreamReader sr = new StreamReader(file))
            {
                emailBody = await sr.ReadToEndAsync();
                emailBody = emailBody.Replace("clientName", obj.clientName);
                if(status == false)
                {
                    emailBody = emailBody.Replace("orderDate", String.Format("{0:dd/MM/yyyy HH:mm}", obj.dateOfReservation));
                    emailBody = emailBody.Replace("tableNumber", obj.tableNumber.ToString());
                }
            }

            var values = new Dictionary<string, string>
            {
               { "email", _email },
               { "subject", "Gustoso Backery" },
               { "body", emailBody }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync($"{_host}/send-email", content);
        }
    }
}
