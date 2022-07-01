//using AdoCurso.API.Models.Entities;
//using AdoCurso.API.Models.Interfaces;
//using AdoCurso.API.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using System;

//namespace AdoCurso.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsuariosController : ControllerBase
//    {
//        #region Propriedades
//        private IUsuarioRepository _repository;
//        #endregion

//        #region Construtor
//        public UsuariosController()
//        {
//            _repository = new UsuarioRepository();
//        }
//        #endregion

//        #region Métodos
//        [HttpGet]
//        [Route("/Buscar")]
//        public IActionResult Get()
//        {
//            return Ok(_repository.GetAll());
//        }

//        [HttpGet]
//        [Route("/Buscar/{id}")]
//        public IActionResult Get(int id)
//        {
//            var usuario = _repository.GetById(id);
//            if (usuario == null)
//            {
//                return NotFound("Não existe usuário com este Id.");
//            }
//            return Ok(usuario);
//        }

//        [HttpPost]
//        [Route("/Cadastrar")]
//        public IActionResult Insert(Usuario usuario)
//        {
//            try
//            {
//                _repository.Insert(usuario);
//                return Ok(usuario);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPut]
//        [Route("/Atualizar/{id}")]
//        public IActionResult Update(Usuario usuario)
//        {
//            try
//            {
//                _repository.Update(usuario);
//                return Ok(usuario);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpDelete]
//        [Route("/Deletar/{id}")]
//        public IActionResult Delete(int id)
//        {
//            _repository.Delete(id);
//            return Ok();
//        }
//        #endregion
//    }
//}
