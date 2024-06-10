using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core.DTOs;
using HotelBooking.Core.Interfaces;
using HotelBooking.Core.Models;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms.Include(r => r.Hotel).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Room>> SearchRoomsAsync(SearchCriteriaDto criteria)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Where(r => r.Hotel.Location == criteria.Destination &&
                            r.IsAvailable &&
                            r.Capacity >= criteria.NumberOfPeople &&
                            !_context.Bookings.Any(b => b.RoomId == r.Id &&
                                ((criteria.StartDate >= b.StartDate && criteria.StartDate <= b.EndDate) ||
                                 (criteria.EndDate >= b.StartDate && criteria.EndDate <= b.EndDate))))
                .ToListAsync();
        }

        public async Task<Room> RegisterRoomAsync(RoomRegisterDto roomDto)
        {
            var room = new Room
            {
                HotelId = roomDto.HotelId,
                Type = roomDto.Type,
                Capacity = roomDto.Capacity,
                Price = roomDto.Price,
                IsAvailable = roomDto.IsAvailable
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return room;
        }

        public async Task<Room> UpdateRoomAsync(int id, Room updatedRoom)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return null;
            }

            room.Type = updatedRoom.Type;
            room.Capacity = updatedRoom.Capacity;
            room.Price = updatedRoom.Price;
            room.IsAvailable = updatedRoom.IsAvailable;

            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return room;
        }

        public async Task DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }
    }
}