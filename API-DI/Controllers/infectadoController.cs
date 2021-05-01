using API_DI.Data.Collections;
using API_DI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;

namespace API_DI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class infectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<infectado> _infectadosCollection;

        public infectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<infectado>(typeof(infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] infectadoDto dto)
        {
            var infectado = new infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<infectado>.Filter.Empty).ToList();

            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtualizarInfectados([FromBody] infectadoDto dto)
        {
            _infectadosCollection.UpdateOne(Builders<infectado>.Filter.Where(_ => _.DataNascimento == dto.DataNascimento), Builders<infectado>.Update.Set("sexo", dto.Sexo));

            var infectados = _infectadosCollection.Find(Builders<infectado>.Filter.Empty).ToList();

            return Ok("Atualizado com sucesso!");
        }

        [HttpDelete("{dataNasc}")]
        public ActionResult Delete(DateTime dataNasc)
        {
            _infectadosCollection.DeleteOne(Builders<infectado>.Filter.Where(_ => _.DataNascimento == dataNasc));

            var infectados = _infectadosCollection.Find(Builders<infectado>.Filter.Empty).ToList();

            return Ok("Dado deletado");
        }
    }
}
