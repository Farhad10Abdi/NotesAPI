﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notes_API.Models;
using Notes_API.Models.Dto;
using Notes_API.Repository.IRepository;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace Notes_API.Controllers
{
    [Route("api/NoteAPI")]
    [ApiController]
    public class NoteAPIController : Controller
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;
        private APIResponse _response;

        public NoteAPIController(INoteRepository noteRepository,IMapper mapper)
        {
            _noteRepository = noteRepository;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllNotes()
        {
            try
            {
                List<Notes> notesList = await _noteRepository.GetAllAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<List<NoteDTO>>(notesList);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add(ex.Message);
            }
            return _response;
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetNote(int id)
        {
            try
            {
                var note = await _noteRepository.GetAsync(u => u.ID == id);
                if(note == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<NoteDTO>(note);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }
    }
}