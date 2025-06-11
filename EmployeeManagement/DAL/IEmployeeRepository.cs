using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.DAL
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee? GetById(int id);
        void Add(Employee emp);
        void Update(Employee emp);
        void Delete(int id);
        List<State> GetAllStates();
        void DeleteMultiple(List<int> ids);
        bool IsNameDuplicate(string name, int? id = null);
    }
}