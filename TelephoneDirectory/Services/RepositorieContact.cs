using TelephoneDirectory.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using TelephoneDirectory.Interface;

namespace TelephoneDirectory.Services
{
    
    public class RepositorieContact : IRepositorieContact
    {
        private readonly string connectionString;

        public RepositorieContact(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Contact contact)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Contact
                                                         (Name,Tel,cel) 
                                                    VALUES (@Name, @Tel, @cel);
                                               select SCOPE_IDENTITY();", contact);
            contact.Id = id;
        }

        //Cuando retorna un tipo de dato se debe poner en el Task Task<bool>
        public async Task<bool> Exist(string Name)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM Contact
                                    WHERE Name = @Name",
                                    new { Name });
            return exist == 1;
        }



        // Obtenemos las cuentas del usuario
        public async Task<IEnumerable<Contact>> getContacts()
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Contact>(@"SELECT Id, Name, Tel, Cel
                                                            FROM Contact");

        }

        public async Task<IEnumerable<Contact>> getContact(string nameContact)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Contact>(@"SELECT Id, Name, Tel, Cel
                                                            FROM Contact
                                                    WHERE Name = @nameContact", 
                                                    new { nameContact });

        }
        // Actualizar
        public async Task Modify(Contact contact)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Contact
                                            SET Name = @Name
                                            WHERE Id = @Id", contact);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        public async Task<Contact> getContactById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Contact>(@"
                                                                SELECT Id, Name, Tel, Cel
                                                                FROM Contact
                                                                WHERE Id = @Id",
                                                                new { id });
        }

        //Eliminar
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Contact WHERE Id = @Id", new { id });
        }

        //Cantidad de contactos
        public async Task<int> CountContact()
        {
            using var connection = new SqlConnection(connectionString);
            int numberContact = await connection.QuerySingleAsync<int>("select COUNT(*) from Contact;");

            return numberContact;
        }
    }
}
