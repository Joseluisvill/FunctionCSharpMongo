using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using MongoDB.Driver;

namespace My.Functions
{
    public static class HttpTriggerTopicos
    {
        [FunctionName("CrearPersona")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var clusterMongo= new MongoClient(System.Environment.GetEnvironmentVariable("MongoDBAtlasCString"));
            var dbase=clusterMongo.GetDatabase("ClaseAzuero");
            var coleccion=dbase.GetCollection<Persona>("Personas");
            /*var juan=new Persona();
            juan.nombre="Juan maria";
            juan.edad=30;

            var maria=new Persona{
                nombre="Maria jose",
                edad=37
            };*/
            String nombre=req.Query["nombre"];
            int edad=Int32.Parse(req.Query["edad"]);

            var maria=new Persona{nombre=nombre,edad=edad};
            try
            {
                //await coleccion.InsertOneAsync(juan);
                await coleccion.InsertOneAsync(maria);

                return new OkObjectResult("Se guardaron los datos");
            }
            catch (System.Exception e)
            {
                
                return new BadRequestObjectResult("Hubo un error al insertar "+e.Message);
            }
        
        }
        [FunctionName("ObtenerPersonas")]
        public static async Task<IActionResult> Obtener(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var clusterMongo= new MongoClient(System.Environment.GetEnvironmentVariable("MongoDBAtlasCString"));
            var dbase=clusterMongo.GetDatabase("ClaseAzuero");
            var coleccion=dbase.GetCollection<Persona>("Personas");

            try
            {
                //Creo una variable tipo filter correspondiente a la coleccion Persona
                var filtro=Builders<Persona>.Filter;
                //para obtener todos los elementos debemos hacer un filtro vacio 
                var resultados= await coleccion.FindAsync<Persona>(filtro.Empty);
                //Retorno la lista de documentos de la coleccion Persona
                return new OkObjectResult(resultados.ToList());
            }
            catch (System.Exception e)
            {
                
                return new BadRequestObjectResult("Hubo un error al insertar "+e.Message);
            }
        
        }

        [FunctionName("ActualizarPersona")]
        public static async Task<IActionResult> Actualizar(
            [HttpTrigger(AuthorizationLevel.Anonymous,"put", Route = null)] HttpRequest req,
            ILogger log)
        {
            int edadAgregar=Int32.Parse(req.Query["edad"]);
            String nombre=req.Query["nombre"];

            var clusterMongo= new MongoClient(System.Environment.GetEnvironmentVariable("MongoDBAtlasCString"));
            var dbase=clusterMongo.GetDatabase("ClaseAzuero");
            var coleccion=dbase.GetCollection<Persona>("Personas");
            
            var filtro=Builders<Persona>.Filter.Eq("edad",edadAgregar);
            //otra forma
            //var filtro =new FilterDefinitionBuilder<Persona>().Where(r=>r.edad==edadAgregar);

            var update=Builders<Persona>.Update.Set("nombre",nombre);


            try
            {
                //Para actualizar uno
                await coleccion.UpdateOneAsync(filtro,update);

                return new OkObjectResult("Se actualizo el dato");
            }
            catch (System.Exception e)
            {
                
                return new BadRequestObjectResult("Hubo un error al actualizar "+e.Message);
            }
        
        }
        [FunctionName("EliminarPersona")]
        public static async Task<IActionResult> eliminarPersona(
            [HttpTrigger(AuthorizationLevel.Anonymous,"delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            int edadAgregar=Int32.Parse(req.Query["edad"]);

            var clusterMongo= new MongoClient(System.Environment.GetEnvironmentVariable("MongoDBAtlasCString"));
            var dbase=clusterMongo.GetDatabase("ClaseAzuero");
            var coleccion=dbase.GetCollection<Persona>("Personas");
            
            var filtro=Builders<Persona>.Filter.Eq("edad",edadAgregar);

            try
            {
                //Para actualizar uno
                await coleccion.DeleteOneAsync(filtro);

                return new OkObjectResult("Se elimino el dato");
            }
            catch (System.Exception e)
            {
                
                return new BadRequestObjectResult("Hubo un error al eliminar "+e.Message);
            }
        
        }
    }
}
