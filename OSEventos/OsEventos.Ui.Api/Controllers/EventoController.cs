using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using OSEventos.DataVO.ValueObjects.CadastroBasico;
using OSEventos.Services.Interfaces.CadastroBasico;

namespace OsEventos.Ui.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EventoController : ControllerBase
    {
        /// <summary>
        /// Declaração das interfaces
        /// </summary>
        private readonly IEventoService _eventoService;

        /// <summary>
        /// Metodo contrutor
        /// </summary>
        /// <param name="EventoService"></param>
        public EventoController(IEventoService EventoService)
        {
            _eventoService = EventoService;
        }

        /// <summary>
        /// Retorna todos Registros
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                var Registros = _eventoService.GetAll();
                return new OkObjectResult(Registros);
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Retorna gegistro pelo Id
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        [HttpGet("{pId}")]
        public ActionResult<string> GetPlanoDespesa(int pId)
        {
            try
            {
                var EventoEntity = _eventoService.GetById(pId);
                return new OkObjectResult(EventoEntity);
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Adiciona registro
        /// </summary>
        /// <param name="eventoVo"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] EventoVo eventoVo)
        {
            try
            {
                if (eventoVo == null) return BadRequest();
                var Evento = _eventoService.Add(eventoVo);

                return Ok(Evento);
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " | " + e.InnerException.Message);
            }
        }

        /// <summary>
        /// Altera registro
        /// </summary>
        /// <param name="eventoVo"></param>
        /// <returns></returns>
        //[Authorize("Bearer")]
        [HttpPut]
        public IActionResult Put([FromBody] EventoVo eventoVo)
        {
            try
            {
                _eventoService.Update(eventoVo);
                return Ok(eventoVo);
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " | " + e.InnerException.Message);
            }
        }

        /// <summary>
        /// Deleta registros pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize("Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromUri] string[] id)
        {
            try
            {
                foreach (var i in id)
                {
                    var Atualizar = _eventoService.GetById(int.Parse(i));
                    if (Atualizar != null)
                    {
                        _eventoService.Remove(Atualizar);
                    }
                }
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}