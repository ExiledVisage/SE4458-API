using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core.DTOs;
using HotelBooking.Core.Interfaces;
using HotelBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("get-all-rooms")]
        public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            var roomDtos = rooms.Select(room => new RoomResponseDto
            {
                Id = room.Id,
                Type = room.Type,
                Price = room.Price,
                IsAvailable = room.IsAvailable
            });
            return Ok(roomDtos);
        }

        [HttpGet("get-room/{id}")]
        public async Task<ActionResult<RoomResponseDto>> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            var roomDto = new RoomResponseDto
            {
                Id = room.Id,
                Type = room.Type,
                Price = room.Price,
                IsAvailable = room.IsAvailable
            };
            return Ok(roomDto);
        }

        [HttpPost("create-room")]
        public async Task<ActionResult<RoomResponseDto>> CreateRoom([FromBody] RoomRegisterDto roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest("Room details are required.");
            }

            var createdRoom = await _roomService.RegisterRoomAsync(roomDto);
            var createdRoomDto = new RoomResponseDto
            {
                Id = createdRoom.Id,
                Type = createdRoom.Type,
                Price = createdRoom.Price,
                IsAvailable = createdRoom.IsAvailable
            };
            return CreatedAtAction(nameof(GetRoomById), new { id = createdRoom.Id }, createdRoomDto);
        }

        [HttpPut("update-room/{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomRegisterDto roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest("Room details are required.");
            }

            var roomToUpdate = await _roomService.GetRoomByIdAsync(id);
            if (roomToUpdate == null)
            {
                return NotFound($"Room with Id = {id} not found.");
            }

            roomToUpdate.Type = roomDto.Type;
            roomToUpdate.Price = roomDto.Price;
            roomToUpdate.IsAvailable = roomDto.IsAvailable;

            await _roomService.UpdateRoomAsync(id, roomToUpdate);
            return NoContent();
        }

        [HttpDelete("delete-room/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound($"Room with Id = {id} not found.");
            }

            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }

        [HttpPost("search-rooms")]
        public async Task<ActionResult<IEnumerable<RoomResponseDto>>> SearchRooms([FromBody] SearchCriteriaDto criteria)
        {
            var rooms = await _roomService.SearchRoomsAsync(criteria);
            var roomDtos = rooms.Select(room => new RoomResponseDto
            {
                Id = room.Id,
                Type = room.Type,
                Price = room.Price,
                IsAvailable = room.IsAvailable
            });
            return Ok(roomDtos);
        }
    }
}