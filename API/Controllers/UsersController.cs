using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    // [EnableCors("MyCorsPolicy")]
     [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
      
        private readonly IUserRepository _userReopsitory ;
        public UsersController(IUserRepository userReopsitory,IMapper mapper)
        {
            _mapper = mapper;
            _userReopsitory = userReopsitory;
          
        }
        [HttpGet]
      
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // var users = await _userReopsitory.GetUsersAsync();
            // var usersToReturn =  _mapper.Map<IEnumerable<MemberDto>>(users);
            var users = await _userReopsitory.GetMembersAsync();
          
            return Ok(users);
        }
        [HttpGet("{username}")]
       
        public async  Task<ActionResult<MemberDto>> GetUser(string username){
            // var user = await _userReopsitory.GetUserByUsernameAsync(username);
            
            // return _mapper.Map<MemberDto>(user);
            return await _userReopsitory.GetMemberAsync(username);
        }
    }
}