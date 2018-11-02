using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enterprise : IEnterprise
{
    private Dictionary<Guid, Employee> employees;

    public Enterprise()
    {
        this.employees = new Dictionary<Guid, Employee>();
    }

    public int Count => this.employees.Count;

    public void Add(Employee employee)
    {
        this.employees[employee.Id] = employee;
    }

    public IEnumerable<Employee> AllWithPositionAndMinSalary(Position position, double minSalary)
    {
        return this.employees.Values.Where(a => a.Position == position && a.Salary >= minSalary);
    }

    public bool Change(Guid guid, Employee employee)
    {
        if (!this.employees.ContainsKey(guid))
        {
            return false;
        }
        var oldEmployee = this.employees[guid];
        oldEmployee.FirstName = employee.FirstName;
        oldEmployee.LastName = employee.LastName;
        oldEmployee.Position = employee.Position;
        oldEmployee.Salary = employee.Salary;
        oldEmployee.HireDate = employee.HireDate;
        return true;
    }

    public bool Contains(Guid guid)
    {
        return this.employees.ContainsKey(guid);
    }

    public bool Contains(Employee employee)
    {
        return this.employees.ContainsKey(employee.Id);
    }

    public bool Fire(Guid guid)
    {
        if (!this.employees.ContainsKey(guid))
        {
            return false;
        }
        this.employees.Remove(guid);
        return true;
    }

    public Employee GetByGuid(Guid guid)
    {
        if (!this.employees.ContainsKey(guid))
        {
            throw new ArgumentException();
        }
        return this.employees[guid];
    }

    public IEnumerable<Employee> GetByPosition(Position position)
    {
        var result = this.employees.Values.Where(a => a.Position == position);

        if (!result.Any()) 
        {
            throw new ArgumentException();
        }
        return result;
    }

    public IEnumerable<Employee> GetBySalary(double minSalary)
    {
        var result = this.employees.Values.Where(a => a.Salary >= minSalary);

        if (!result.Any())
        {
            throw new InvalidOperationException();
        }
        return result;
    }

    public IEnumerable<Employee> GetBySalaryAndPosition(double salary, Position position)
    {
        var result = this.employees.Values.Where(a => a.Salary == salary && a.Position == position);

        if (!result.Any())
        {
            throw new InvalidOperationException();
        }
        return result;
    }

    public IEnumerator<Employee> GetEnumerator()
    {
        foreach (var kvp in this.employees)
        {
            yield return kvp.Value;
        }
    }

    public Position PositionByGuid(Guid guid)
    {
        if (!this.employees.ContainsKey(guid))
        {
            throw new InvalidOperationException();
        }

        return this.employees[guid].Position;
    }

    public bool RaiseSalary(int months, int percent)
    {
        var isRaised = false;
        foreach (var kvp in this.employees)
        {
            if (kvp.Value.HireDate.AddMonths(months) <= DateTime.Now)
            {
                kvp.Value.Salary += kvp.Value.Salary * (percent / 100d);
                isRaised = true;
            }
        }

        return isRaised;
    }

    public IEnumerable<Employee> SearchByFirstName(string firstName)
    {
        return this.employees.Values.Where(a => a.FirstName == firstName);
    }

    public IEnumerable<Employee> SearchByNameAndPosition(string firstName, string lastName, Position position)
    {
        return this.employees.Values
            .Where(a => a.FirstName == firstName && a.LastName == lastName && a.Position == position);
    }

    public IEnumerable<Employee> SearchByPosition(IEnumerable<Position> positions)
    {
        return this.employees.Values.Where(a => positions.Contains(a.Position));
    }

    public IEnumerable<Employee> SearchBySalary(double minSalary, double maxSalary)
    {
        return this.employees.Values.Where(a => a.Salary >= minSalary && a.Salary <= maxSalary);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}