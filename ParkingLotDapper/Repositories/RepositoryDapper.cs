using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace ParkingLotDapper.Repositories
{
    public class RepositoryDapper<T> : IRepository<T>
    {

        private readonly IDbConnection _connection;
        private readonly string _tableName;

        public RepositoryDapper(IDbConnection connection)
        {
            _connection = connection;
            _tableName = GetTableName();
        }

        private string GetTableName()
        {
            // Obtém o nome da tabela usando reflexão
            var tipo = typeof(T);
            var tableAttribute = tipo.GetCustomAttribute<TableAttribute>();
            if (tableAttribute != null)
            {
                return tableAttribute.Name;
            }
            return $"{tipo.Name}s";
        }

        public IEnumerable<T> FindAll()
        {
            var sql = $"SELECT * FROM {_tableName}";
            return _connection.Query<T>(sql);
        }

        public T FindById(int id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE id = @id";
            return _connection.QueryFirstOrDefault<T>(sql, new { Id = id })!;
        }

        public void Create(T entity)
        {
            var fields = GetCreateFields(entity);
            var values = GetCreatetValues(entity);
            var sql = $"INSERT INTO {_tableName} ({fields}) VALUES ({values})";
            _connection.Execute(sql, entity);
        }

        public void Update(T entity)
        {
            var fields = GetUpdateField(entity);
            var sql = $"UPDATE {_tableName} SET {fields} WHERE Id = @Id";
            _connection.Execute(sql, entity);
        }

        public void Delete(int id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }

        private string GetCreateFields(T entity)
        {
            var tipo = typeof(T);
            var properties = tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !Attribute.IsDefined(p, typeof(IgnoreInDapperAttribute)));

            var fieldsNames = properties.Select(p =>
            {
                var columnName = p.GetCustomAttribute<ColumnAttribute>()?.Name;
                return columnName ?? p.Name;
            });

            return string.Join(", ", fieldsNames);
        }

        private string GetCreatetValues(T entity)
        {
            var tipo = typeof(T);
            var properties = tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !Attribute.IsDefined(p, typeof(IgnoreInDapperAttribute)));

            var fieldsNames = properties.Select(p =>
            {
                var columnName = p.GetCustomAttribute<ColumnAttribute>()?.Name;
                return columnName ?? p.Name;
            });

            return string.Join(", ", fieldsNames.Select(p => $"@{p}"));
        }

        private string GetUpdateField(T entidade)
        {
            var tipo = typeof(T);
            var properties = tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !Attribute.IsDefined(p, typeof(IgnoreInDapperAttribute)));

            var updateFields = properties.Select(p =>
            {
                var columnName = p.GetCustomAttribute<ColumnAttribute>()?.Name;
                return $"{columnName ?? p.Name} = @{p.Name}";
            });

            return string.Join(", ", updateFields);
        }
    }
}
