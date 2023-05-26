﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Notes_API.Models;
using Notes_API.Models.Dto;
using Notes_API.Repository.IRepository;
using System.Net;

namespace Notes_API.Controllers
{
    [Route("api/NoteBookAPI")]
    [ApiController]
    public class NoteBookAPIController : ControllerBase
    {
        private readonly INoteBookRepository _noteBookRepository;
        private readonly IMapper _mapper;
        private APIResponse _response;
        public NoteBookAPIController(INoteBookRepository noteBookRepository, IMapper mapper)
        {
            _noteBookRepository = noteBookRepository;
            _mapper = mapper;
            this._response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllNoteBooks()
        {
            try
            {
                List<NoteBook> noteBooks = await _noteBookRepository.GetAllAsync();
                if (noteBooks.Count == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Nothing found !");
                    return NotFound(_response);
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<List<NoteBookDTO>>(noteBooks);
                
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add(ex.Message);
                return BadRequest(_response);
            }
        }
    }
}