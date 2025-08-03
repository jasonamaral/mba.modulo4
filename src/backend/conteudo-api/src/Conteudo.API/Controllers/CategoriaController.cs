using Conteudo.API.Controllers.Base;
using Conteudo.Application.Commands;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Mapster;

namespace Conteudo.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class CategoriaController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly ICategoriaAppService _categoriaAppService;
        
        public CategoriaController(INotificador notificador
                                  , IMediatorHandler mediator
                                  , ICategoriaAppService categoriaAppService) : base(notificador)
        {
            _mediator = mediator;
            _categoriaAppService = categoriaAppService;
        }

        /// <summary>
        /// Retorna uma categoria pelo ID.
        /// </summary>
        /// <param name="id">ID do curso</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiSuccess), 200)]
        [ProducesResponseType(typeof(ResponseResult), 404)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var categoria = await _categoriaAppService.ObterPorIdAsync(id);
             
                if (categoria == null)
                    return RespostaPadraoApi(HttpStatusCode.NotFound, "Categoria não encontrada.");

                return RespostaPadraoApi(data: categoria);
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Retorna todas as categorias.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), 200)]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var categorias = await _categoriaAppService.ObterTodasCategoriasAsync();
                return RespostaPadraoApi(data: categorias);
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Cadastra uma nova categoria.
        /// </summary>
        /// <param name="dto">Dados da categoria</param>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(ApiSuccess), 201)]
        [ProducesResponseType(typeof(ResponseResult), 400)]
        public async Task<IActionResult> CadastrarCategoria([FromBody] CadastroCategoriaDto dto)
        {
            try
            {
                var command = dto.Adapt<CadastrarCategoriaCommand>();
                return RespostaPadraoApi(HttpStatusCode.Created, await _mediator.EnviarComando(command));
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
