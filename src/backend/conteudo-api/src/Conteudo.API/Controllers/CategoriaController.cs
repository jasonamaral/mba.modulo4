using AutoMapper;
using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Services.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Conteudo.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class CategoriaController(IMediatorHandler mediator
                                  , ICategoriaAppService categoriaAppService
                                  , IMapper mapper
                                  , INotificationHandler<DomainNotificacaoRaiz> notifications) : MainController(mediator, notifications)
    {
        private readonly IMediatorHandler _mediator = mediator;
        private readonly ICategoriaAppService _categoriaAppService = categoriaAppService;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Retorna uma categoria pelo ID.
        /// </summary>
        /// <param name="id">ID do curso</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseResult<CategoriaDto>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 404)]
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
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
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
        [ProducesResponseType(typeof(ResponseResult<Guid>), 201)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        public async Task<IActionResult> CadastrarCategoria([FromBody] CadastroCategoriaDto dto)
        {
            try
            {
                var command = _mapper.Map<CadastrarCategoriaCommand>(dto);
                return RespostaPadraoApi(HttpStatusCode.Created, await _mediator.ExecutarComando(command));
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
