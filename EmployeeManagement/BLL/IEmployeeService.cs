using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.BLL
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();
        List<State> GetAllStates();
        Employee? Get(int id);
        void Create(Employee emp); 
        void Edit(Employee emp); 
        void Remove(int id);

        void RemoveMultiple(List<int> ids);
        void ValidateAndCreate(Employee emp); 
        void ValidateAndEdit(Employee emp);  
    }
}