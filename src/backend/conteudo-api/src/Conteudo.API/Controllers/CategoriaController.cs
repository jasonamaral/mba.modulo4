using AutoMapper;
using Conteudo.API.Controllers.Base;
using Conteudo.Application.Commands;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conteudo.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class CategoriaController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly ICategoriaAppService _categoriaAppService;
        private readonly IMapper _mapper;
        public CategoriaController(INotificador notificador
                                  , IMediatorHandler mediator
                                  , ICategoriaAppService categoriaAppService
                                  , IMapper mapper) : base(notificador)
        {
            _mediator = mediator;
            _categoriaAppService = categoriaAppService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna uma categoria pelo ID.
        /// </summary>
        /// <param name="id">ID do curso</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiSuccess), 200)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var categoria = await _categoriaAppService.ObterPorIdAsync(id);
             
                if (categoria == null)
                    return NotFound(new ApiError { Message = "Categoria não encontrada." });

                return RespostaPadraoApi(data: categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
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
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        /// <summary>
        /// Cadastra uma nova categoria.
        /// </summary>
        /// <param name="dto">Dados da categoria</param>
        /// Retorna `ApiSuccess` em caso de sucesso ou `ApiError` em caso de erro.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiSuccess), 201)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> CadastrarCategoria([FromBody] CadastroCategoriaDto dto)
        {
            try
            {
                var command = _mapper.Map<CadastrarCategoriaCommand>(dto);
                return RespostaPadraoApi(await _mediator.EnviarComando(command));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }
    }
}
