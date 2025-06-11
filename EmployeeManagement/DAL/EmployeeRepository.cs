using EmployeeManagement.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic; 
using System.Linq; 

namespace EmployeeManagement.DAL;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IConfiguration _config;
    private readonly string _connStr;

    public EmployeeRepository(IConfiguration config)
    {
        _config = config;
        _connStr = _config.GetConnectionString("DefaultConnection")!;
    }

    public List<Employee> GetAll()
    {
        var list = new List<Employee>();
        using var conn = new MySqlConnection(_connStr);
        using var cmd = new MySqlCommand("SELECT * FROM Employees", conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(ReadEmployee(reader));
        }
        return list;
    }

    public List<State> GetAllStates()
    {
        var list = new List<State>();
        using var conn = new MySqlConnection(_connStr);
        using var cmd = new MySqlCommand("SELECT * FROM States", conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new State
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString()!
            });
        }
        return list;
    }

    public Employee? GetById(int id)
    {
        using var conn = new MySqlConnection(_connStr);
        using var cmd = new MySqlCommand("SELECT * FROM Employees WHERE Id=@Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? ReadEmployee(reader) : null;
    }

    public void Add(Employee emp)
    {
        using var conn = new MySqlConnection(_connStr);
        using var cmd = new MySqlCommand("INSERT INTO Employees (Name, Designation, DateOfJoin, DateOfBirth, Salary, Gender, State) VALUES (@Name, @Designation, @DOJ, @DOB, @Salary, @Gender, @State)", conn);
        SetParams(cmd, emp);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Update(Employee emp)
    {
        using var conn = new MySqlConnection(_connStr);
        using var cmd = new MySqlCommand("UPDATE Employees SET Name=@Name, Designation=@Designation, DateOfJoin=@DOJ, DateOfBirth=@DOB, Salary=@Salary, Gender=@Gender, State=@State WHERE Id=@Id", conn);
        SetParams(cmd, emp);
        cmd.Parameters.AddWithValue("@Id", emp.Id);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = new MySqlConnection(_connStr);
        using var cmd = new MySqlCommand("DELETE FROM Employees WHERE Id=@Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    
    public void DeleteMultiple(List<int> ids)
    {
        if (ids == null || !ids.Any())
            return;

     
        var idList = string.Join(",", ids);
        using var conn = new MySqlConnection(_connStr);
        using var cmd = new MySqlCommand($"DELETE FROM Employees WHERE Id IN ({idList})", conn);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public bool IsNameDuplicate(string name, int? id = null)
    {
        using var conn = new MySqlConnection(_connStr);
        var query = "SELECT COUNT(*) FROM Employees WHERE Name = @Name";
        if (id.HasValue && id.Value > 0)
        {
            query += " AND Id != @Id";
        }
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Name", name);
        if (id.HasValue && id.Value > 0)
        {
            cmd.Parameters.AddWithValue("@Id", id.Value);
        }
        conn.Open();
        var count = Convert.ToInt32(cmd.ExecuteScalar());
        return count > 0;
    }


    private Employee ReadEmployee(IDataReader r)
    {
        return new Employee
        {
            Id = Convert.ToInt32(r["Id"]),
            Name = r["Name"].ToString()!,
            Designation = r["Designation"].ToString()!,
            DateOfJoin = Convert.ToDateTime(r["DateOfJoin"]),
            DateOfBirth = Convert.ToDateTime(r["DateOfBirth"]),
            Salary = Convert.ToDecimal(r["Salary"]),
            Gender = r["Gender"].ToString()!,
            State = r["State"].ToString()!
        };
    }

    private void SetParams(MySqlCommand cmd, Employee emp)
    {
        cmd.Parameters.AddWithValue("@Name", emp.Name);
        cmd.Parameters.AddWithValue("@Designation", emp.Designation);
        cmd.Parameters.AddWithValue("@DOJ", emp.DateOfJoin);
        cmd.Parameters.AddWithValue("@DOB", emp.DateOfBirth);
        cmd.Parameters.AddWithValue("@Salary", emp.Salary);
        cmd.Parameters.AddWithValue("@Gender", emp.Gender);
        cmd.Parameters.AddWithValue("@State", emp.State);
    }
}