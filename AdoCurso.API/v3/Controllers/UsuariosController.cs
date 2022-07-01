using AdoCurso.API.Models.Entities;
using AdoCurso.API.Models.Interfaces.v3;
using AdoCurso.API.Repositories.v3;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AdoCurso.API.v3.Controllers
{
    [ApiController]
    [ApiVersion("3.0")]
    public class UsuariosController : ControllerBase
    {
        #region Propriedades
        private readonly IUsuarioRepositoryV3 _repository;
        #endregion

        #region Construtor
        public UsuariosController()
        {
            _repository = new UsuarioRepositoryV3();
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

        [HttpPut]
        [Route("api/v{version:ApiVersion}/Usuarios/Atualizar/{id}")]
        public IActionResult Update(Usuario usuario)
        {
            try
            {
                _repository.Update(usuario);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/v{version:ApiVersion}/Usuarios/Deletar/{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return Ok();
        }
        #endregion
    }
}
