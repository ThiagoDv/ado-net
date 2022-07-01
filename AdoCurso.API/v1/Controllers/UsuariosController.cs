using AdoCurso.API.Models.Entities;
using AdoCurso.API.Models.Interfaces.v1;
using AdoCurso.API.Repositories.v1;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AdoCurso.API.v1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    public class UsuariosController : ControllerBase
    {
        #region Propriedades
        private readonly IUsuarioRepositoryV1 _repository;
        #endregion

        #region Construtor
        public UsuariosController()
        {
            _repository = new UsuarioRepositoryV1();
        }
        #endregion

        #region Métodos
        [HttpGet]
        [Route("api/v{version:ApiVersion}/Usuarios/Buscar")]
        public IActionResult Get()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet]
        [Route("api/v{version:ApiVersion}/Usuarios/Buscar/{id}")]
        public IActionResult Get(int id)
        {
            var usuario = _repository.GetById(id);
            if (usuario == null)
            {
                return NotFound("Não existe usuário com este Id.");
            }
            return Ok(usuario);
        }

        [HttpPost]
        [Route("api/v{version:ApiVersion}/Usuarios/Cadastrar")]
        public IActionResult Insert(Usuario usuario)
        {
            try
            {
                _repository.Insert(usuario);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
