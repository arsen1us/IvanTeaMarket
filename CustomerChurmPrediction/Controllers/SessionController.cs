﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Entities.SessionEntity;
using CustomerChurmPrediction.Services;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController(
        ISessionService _sessionService,
        IUserService _userService) : Controller
    {
        /// <summary>
        /// Создание сессии
        /// </summary>
        // POST: /api/session

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpPost]
        public async Task<IActionResult> CreateSessionAsync(SessionAdd sessionAdd)
        {
            if (sessionAdd is null || string.IsNullOrEmpty(sessionAdd.UserId))
                return BadRequest();
            try
            {
                var user = await _userService.FindByIdAsync(sessionAdd.UserId);

                var session = new Session
                {
                    UserId = sessionAdd.UserId,
                    SessionTimeStart = sessionAdd.SessionTimeStart,
                    CreateTime = sessionAdd.SessionTimeStart,

                    CreatorId = sessionAdd.UserId,
                    UserIdLastUpdate = sessionAdd.UserId,

                };

                bool isSuccess = await _sessionService.SaveOrUpdateAsync(session, default);

                if (isSuccess)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        /// <summary>
        /// Обновление времени сессии
        /// </summary>
        // PUT: api/session/{userId}

        [Authorize(Roles = "User, Admin, Owner")]
        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateSessionAsync(string userId, SessionUpdate sessionUpdate)
        {
            if (sessionUpdate is null)
                return BadRequest();
            try
            {
                var lastSession = await _sessionService.GetLastByUserIdAsync(userId, default);

                if (lastSession is null) return NotFound();

                lastSession.SessionTimeEnd = sessionUpdate.UpdateTime;

                bool isSuccess = await _sessionService.SaveOrUpdateAsync(lastSession, default);

                if (isSuccess)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
