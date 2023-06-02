using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Notes_API.Models;
using Notes_API.Models.Dto;
using Notes_API.Repository.IRepository;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text.Json;

namespace Notes_API.Controllers.v1
{
    [Route("api/v{version:apiversion}/NoteAPI")]
    [ApiController]
    public class NoteAPIController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;
        private APIResponse _response;

        public NoteAPIController(INoteRepository noteRepository, IMapper mapper)
        {
            _noteRepository = noteRepository;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        [ResponseCache(Duration =30)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllNotes(int pageSize = 2 , int pageNumber = 1)
        {
            try
            {
                List<Notes> notesList = await _noteRepository.GetAllAsync(pageSize:pageSize , pageNumber:pageNumber);


                Pagination pagination = new() { pageNumber = pageNumber, pageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<List<NoteDTO>>(notesList);
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


        [HttpGet("{id:int}")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetNote(int id)
        {
            try
            {
                var note = await _noteRepository.GetAsync(u => u.ID == id);
                if (note == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Didn't find the resource !");
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<NoteDTO>(note);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> CreateNote([FromBody] NoteCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Please provide valid information !");
                    return BadRequest(_response);
                }
                Notes note = _mapper.Map<Notes>(createDTO);
                await _noteRepository.CreateAsync(note);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.result = _mapper.Map<NoteDTO>(note);

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

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteNote(int id)
        {
            try
            {
                Notes note = await _noteRepository.GetAsync(u => u.ID == id);
                if (note == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Didn't find the resource !");
                    return BadRequest(_response);
                }
                await _noteRepository.RemoveAsync(note);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateNote([FromBody] NoteUpdateDTO updateDTO)
        {
            try
            {
                Notes note = await _noteRepository.GetAsync(u => u.ID == updateDTO.ID, tracked: false);
                if (note == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Didn't find the resource !");
                    return BadRequest(_response);
                }
                note = _mapper.Map<Notes>(updateDTO);
                await _noteRepository.UpdateAsync(note);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<NoteDTO>(note);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdatePartialNote(int id, JsonPatchDocument<NoteUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Please provide valid informations !");
                    return BadRequest(_response);
                }
                var note = await _noteRepository.GetAsync(u => u.ID == id, tracked: false);

                if (note == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("The item you looking for is not found !");
                    return BadRequest(_response);
                }

                NoteUpdateDTO NoteDTO = _mapper.Map<NoteUpdateDTO>(note);

                patchDTO.ApplyTo(NoteDTO, ModelState);

                Notes model = _mapper.Map<Notes>(NoteDTO);

                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(ModelState);
                }

                await _noteRepository.UpdateAsync(model);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = model;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add(ex.Message);
            }
            return Ok(_response);
        }
    }
}
