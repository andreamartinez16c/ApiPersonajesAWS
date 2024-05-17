using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace ApiPersonajesAWS.Repositories
{
    public class RepositoryPersonajes
    {
        private PersonajesContext context;

        public RepositoryPersonajes(PersonajesContext context)
        {
            this.context = context;
        }

        public async Task<List<Personaje>>GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }

        public async Task<Personaje> FindPersonajeAsync(int id)
        {
            return await this.context.Personajes.FirstOrDefaultAsync(x => x.IdPersonaje == id);
        }

        private async Task<int> GetMaxIdPersonajeAsync()
        {
            return await this.context.Personajes.MaxAsync(x => x.IdPersonaje) + 1;
        }

        public async Task CreatePersonajeAsync
            (string nombre, string imagen)
        {
            Personaje personaje = new Personaje
            {
                IdPersonaje = await this.GetMaxIdPersonajeAsync(),
                Nombre = nombre,
                Imagen = imagen
            };
            this.context.Personajes.Add(personaje);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdatePersonajeAsync(int idPersonaje, string nombre, string imagen)
        {
            var pIdPersonaje = new MySqlParameter("@p_IdPersonaje", idPersonaje);
            var pNombre = new MySqlParameter("@p_Nombre", nombre);
            var pImagen = new MySqlParameter("@p_Imagen", imagen);

            await this.context.Database.ExecuteSqlRawAsync(
                "CALL SP_UPDATE_PERSONAJE(@p_IdPersonaje, @p_Nombre, @p_Imagen)",
                pIdPersonaje, pNombre, pImagen
            );
        }
    }
}
