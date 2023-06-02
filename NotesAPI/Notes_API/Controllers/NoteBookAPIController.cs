using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Notes_API.Models;
using Notes_API.Models.Dto;
using Notes_API.Repository.IRepository;
using System.Net;

namespace Notes_API.Controllers
{
    [Route("api/v{version:apiversion}/NoteBookAPI")]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNoteBook(int id)
        {
            try
            {
                NoteBook noteBook = await _noteBookRepository.GetAsync(u => u.Id == id);
                if (noteBook == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Nothing found !");
                    return NotFound(_response);
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<NoteBookDTO>(noteBook);

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


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateNoteBook([FromBody] NoteBookCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Please provide valid informations !");
                    return BadRequest(_response);
                }
                NoteBook noteBook = _mapper.Map<NoteBook>(createDTO);
                
                await _noteBookRepository.CreateAsync(noteBook);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<NoteBookDTO>(noteBook);
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
        public async Task<ActionResult<APIResponse>> DeleteNoteBook(int id)
        {
            try
            {
                NoteBook noteBook = await _noteBookRepository.GetAsync(u => u.Id == id);
                if (noteBook == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Nothing found to delete !");
                    return NotFound(_response);
                }

                await _noteBookRepository.RemoveAsync(noteBook);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

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


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateNoteBook(int id , [FromBody] NoteBookUpdateDTO updateDTO)
        {
            try
            {
                if (id != updateDTO.Id || updateDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Please provide valid informations !");
                    return BadRequest(_response);
                }
                NoteBook noteBook = await _noteBookRepository.GetAsync(u => u.Id == id, tracked:false);
                if (noteBook == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Nothing found to update !");
                    return NotFound(_response);
                }
                noteBook = _mapper.Map<NoteBook>(updateDTO);
                await _noteBookRepository.UpdateAsync(noteBook);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<NoteBookDTO>(noteBook);

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


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePartialNoteBook (int id , JsonPatchDocument<NoteBookUpdateDTO> patchDTO)
        {
            try
            {
                if (id == 0 || patchDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Please provide valid information !");
                    return BadRequest(_response);
                }
                NoteBook noteBook = await _noteBookRepository.GetAsync(u => u.Id == id , tracked:false);
                if (noteBook == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Nothing found to update !");
                    return NotFound(_response);
                }
                NoteBookUpdateDTO updateDTO = _mapper.Map<NoteBookUpdateDTO>(noteBook);

                patchDTO.ApplyTo(updateDTO, ModelState);

                NoteBook model = _mapper.Map<NoteBook>(updateDTO);

                await _noteBookRepository.UpdateAsync(model);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.result = _mapper.Map<NoteBookDTO>(model);

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
