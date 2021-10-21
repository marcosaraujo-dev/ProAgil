using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain.Model;
using ProAgil.Repository.Interfaces;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController: ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public EventoController(IProAgilRepository repo)
        {
            _repo = repo;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repo.GetAllEventoAsync(true);
                return Ok(results);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,$"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var results = await _repo.GetAllEventoAsyncById(eventoId, true);
                return Ok(results);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,$"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var results = await _repo.GetAllEventoAsyncByTema(tema, true);
                return Ok(results);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,$"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

        }
        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                _repo.Add(model);
                if(await _repo.SaveChangesAsync()){
                    return Created($"api/evento/{model.Id}", model);
                };
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,$"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }

            return BadRequest();

        }

        [HttpPut]
        public async Task<IActionResult> Put(int eventoId, Evento model)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(eventoId, false);

                if(evento == null) return NotFound();

                _repo.Update(model);
                
                if(await _repo.SaveChangesAsync()){
                    return Created($"api/evento/{model.Id}", model);
                };
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,$"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }
            
            return BadRequest();

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(eventoId, false);

                if(evento == null) return NotFound();

                _repo.Delete(evento);
                
                if(await _repo.SaveChangesAsync()){
                    return NoContent();
                };
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,$"Ocorreu problemas na comunicação com o Banco de dados.({ex.Message})");
            }
            
            return BadRequest();

        }
    }
}