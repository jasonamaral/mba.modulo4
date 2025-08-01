using BFF.API.Models.Request;
using BFF.API.Services;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFF.API.Controllers
{
    /// <summary>
    /// Controller de Conteudos no BFF - Orquestra chamadas para Conteudo.API
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConteudosController : ControllerBase
    {
        private readonly IConteudoService _conteudoService;
        private readonly ICacheService _cacheService;
        public ConteudosController(IConteudoService conteudoService, ICacheService cacheService)
        {
            _conteudoService = conteudoService;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Obter dados de um curso pelo Id
        /// </summary>
        [HttpGet("cursos/{cursoId:guid}")]
        public async Task<IActionResult> ObterCursoPorId(Guid cursoId)
        {
            if (cursoId == Guid.Empty)
            {
                return BadRequest("Id do curso inválido.");
            }
            var cacheKey = $"Curso_{cursoId}";
            var cachedCurso = await _cacheService.GetAsync<CursoDto>(cacheKey);
            if (cachedCurso != null)
            {
                return Ok(cachedCurso);
            }

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var curso = await _conteudoService.ObterCursoPorId(cursoId, token);
            if (curso == null)
            {
                return NotFound($"Curso com Id {cursoId} não encontrado.");
            }
            await _cacheService.SetAsync(cacheKey, curso, TimeSpan.FromMinutes(30));
            return Ok(curso);
        }

        /// <summary>
        /// Obter todos os cursos
        /// </summary>
        [HttpGet("cursos")]
        public async Task<IActionResult> ObterTodosCursos()
        {
            var cacheKey = "TodosCursos";
            var cachedCursos = await _cacheService.GetAsync<IEnumerable<CursoDto>>(cacheKey);
            if (cachedCursos != null)
            {
                return Ok(cachedCursos);
            }

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var cursos = await _conteudoService.ObterTodosCursos(token);
            if (cursos == null)
            {
                return NotFound("Nenhum curso encontrado.");
            }
            await _cacheService.SetAsync(cacheKey, cursos, TimeSpan.FromMinutes(30));
            return Ok(cursos);
        }

        /// <summary>
        /// Cadastrar um novo curso
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpPost("cursos")]
        public async Task<IActionResult> AdicionarCurso([FromBody] CursoCriarRequest curso)
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var response = await _conteudoService.AdicionarCurso(curso, token);
            if (response == null)
            {
                return StatusCode(500, "Erro ao adicionar curso.");
            }
            return CreatedAtAction(nameof(AdicionarCurso), new { cursoId = response.Data }, response);
        }

        /// <summary>
        /// Atualizar um curso existente
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpPut("cursos/{cursoId}")]
        public async Task<IActionResult> AtualizarCurso(Guid cursoId, [FromBody] AtualizarCursoRequest curso)
        {
            if (cursoId == Guid.Empty)
            {
                return BadRequest("Id do curso inválido.");
            }

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var response = await _conteudoService.AtualizarCurso(cursoId, curso, token);
            if (response == null)
            {
                return NotFound($"Curso com Id {cursoId} não encontrado.");
            }
            return Ok(response);
        }
    }
}
