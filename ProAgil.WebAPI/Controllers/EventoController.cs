using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain.Model;
using ProAgil.Repository.Interfaces;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        private readonly IMapper _mapper;

        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsync(true);
                
                var results = _mapper.Map<EventoDto[]>(eventos);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(eventoId, true);
                 var results = _mapper.Map<EventoDto>(evento);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsyncByTema(tema, true);
                 var results = _mapper.Map<EventoDto[]>(eventos);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

        }
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                _repo.Add(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"api/evento/{evento.Id}", _mapper.Map<EventoDto>(evento));
                };
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

            return BadRequest();

        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(eventoId, false);

                if (evento == null) return NotFound();

                _mapper.Map(model, evento);

                _repo.Update(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                };
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

            return BadRequest();

        }

        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(eventoId, false);

                if (evento == null) return NotFound();

                _repo.Delete(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return NoContent();
                };
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

            return BadRequest();

        }
    }
}