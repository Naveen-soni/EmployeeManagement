using EmployeeManagement.DAL;
using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.BLL;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;

    public EmployeeService(IEmployeeRepository repo)
    {
        _repo = repo;
    }

    public List<Employee> GetAll() => _repo.GetAll();
    public List<State> GetAllStates() => _repo.GetAllStates();
    public Employee? Get(int id) => _repo.GetById(id);

    public void Create(Employee emp) => _repo.Add(emp);
    public void Edit(Employee emp) => _repo.Update(emp);
    public void Remove(int id) => _repo.Delete(id);

    public void RemoveMultiple(List<int> ids)
    {
        _repo.DeleteMultiple(ids);
    }

    public void ValidateAndCreate(Employee emp)
    {
        if (_repo.IsNameDuplicate(emp.Name))
        {
            throw new InvalidOperationException("Employee with this name already exists.");
        }
        _repo.Add(emp);
    }

    public void ValidateAndEdit(Employee emp)
    {
        _repo.Update(emp);
    }
}