using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Enterprise : IEnterprise
{
    private HashSet<Employee> employees;
    private Dictionary<Guid, Employee> byGuid;
    private OrderedDictionary<DateTime, HashSet<Employee>> byDate;
    private Dictionary<Position, HashSet<Employee>> byPosition;
    private OrderedDictionary<double, HashSet<Employee>> bySalary;
    private Dictionary<Position, OrderedDictionary<double, HashSet<Employee>>> byPositionAndSalary;
    private Dictionary<string, HashSet<Employee>> byFirstName;
    private Dictionary<Position, Dictionary<string, HashSet<Employee>>> byPositionAndName;

    public Enterprise()
    {
        this.employees = new HashSet<Employee>();
        this.byGuid = new Dictionary<Guid, Employee>();
        this.byDate = new OrderedDictionary<DateTime, HashSet<Employee>>();
        this.byPosition = new Dictionary<Position, HashSet<Employee>>();
        this.bySalary = new OrderedDictionary<double, HashSet<Employee>>();
        this.byPositionAndSalary = new Dictionary<Position, OrderedDictionary<double, HashSet<Employee>>>();
        this.byFirstName = new Dictionary<string, HashSet<Employee>>();
        this.byPositionAndName = new Dictionary<Position, Dictionary<string, HashSet<Employee>>>();
    }

    public int Count => this.employees.Count;

    public void Add(Employee employee)
    {
        this.employees.Add(employee);
        this.byGuid[employee.Id] = employee;
        this.AddByDate(employee);
        this.AddByPosition(employee);
        this.AddBySalary(employee);
        this.AddByPositionAndSalary(employee);
        this.AddByFirstName(employee);
        this.AddByPositionAndName(employee);
    }

    public IEnumerable<Employee> AllWithPositionAndMinSalary(Position position, double minSalary)
    {
        if (!this.byPositionAndSalary.ContainsKey(position))
        {
            return Enumerable.Empty<Employee>();
        }

        var range = this.byPositionAndSalary[position].RangeTo(minSalary, true);
        var linkedList = new LinkedList<Employee>();

        foreach (var kvp in range)
        {
            foreach (var employee in kvp.Value)
            {
                linkedList.AddLast(employee);
            }
        }

        return linkedList;
    }

    public bool Change(Guid guid, Employee employee)
    {
        if (!this.byGuid.ContainsKey(guid))
        {
            return false;
        }
        this.byGuid[guid] = employee;
        return true;
    }

    public bool Contains(Guid guid)
    {
        return this.byGuid.ContainsKey(guid);
    }

    public bool Contains(Employee employee)
    {
        return this.employees.Contains(employee);
    }

    public bool Fire(Guid guid)
    {
        if (!this.byGuid.ContainsKey(guid))
        {
            return false;
        }
        var currentEmployee = this.byGuid[guid];
        var fullName = this.GetFullName(currentEmployee);
        this.byGuid.Remove(guid);
        this.employees.Remove(currentEmployee);
        this.byDate[currentEmployee.HireDate].Remove(currentEmployee);
        this.byPosition[currentEmployee.Position].Remove(currentEmployee);
        this.bySalary[currentEmployee.Salary].Remove(currentEmployee);
        this.byPositionAndSalary[currentEmployee.Position][currentEmployee.Salary].Remove(currentEmployee);
        this.byFirstName[currentEmployee.FirstName].Remove(currentEmployee);
        this.byPositionAndName[currentEmployee.Position][fullName].Remove(currentEmployee);

        return true;
    }

    public Employee GetByGuid(Guid guid)
    {
        if (!this.byGuid.ContainsKey(guid))
        {
            throw new ArgumentException();
        }

        return this.byGuid[guid];
    }

    public IEnumerable<Employee> GetByPosition(Position position)
    {
        if (!this.byPosition.ContainsKey(position))
        {
            throw new ArgumentException();
        }

        return this.byPosition[position];
    }

    public IEnumerable<Employee> GetBySalary(double minSalary)
    {
        var range = this.bySalary.RangeFrom(minSalary, true);

        if (range.Count == 0)
        {
            throw new InvalidOperationException();
        }

        var linkedList = new LinkedList<Employee>();
        foreach (var kvp in range)
        {
            foreach (var employee in kvp.Value)
            {
                linkedList.AddLast(employee);
            }
        }

        return linkedList;
    }

    public IEnumerable<Employee> GetBySalaryAndPosition(double salary, Position position)
    {
        if (!this.byPositionAndSalary.ContainsKey(position) || !this.byPositionAndSalary[position].ContainsKey(salary))
        {
            throw new InvalidOperationException();
        }

        return this.byPositionAndSalary[position][salary];
    }

    public IEnumerator<Employee> GetEnumerator()
    {
        foreach (var employee in this.employees)
        {
            yield return employee;
        }
    }

    public Position PositionByGuid(Guid guid)
    {
        if (!this.byGuid.ContainsKey(guid))
        {
            throw new InvalidOperationException();
        }

        return this.byGuid[guid].Position;
    }

    public bool RaiseSalary(int months, int percent)
    {
        var range = this.byDate.Range(DateTime.MinValue, true, DateTime.Now.AddMonths(-months), true);

        if (range.Count == 0)
        {
            return false;
        }

        foreach (var kvp in range)
        {
            foreach (var employee in kvp.Value)
            {     
                this.UpdateSalary(employee, percent);
            }
        }

        return true;
    }

    private void UpdateSalary(Employee employee, int percent)
    {
        var newSalary = employee.Salary + (employee.Salary * (percent / 100d));
        this.UpdateBySalary(employee, newSalary);
        this.UpdateByPositionAndSalary(employee, newSalary);
        employee.Salary = newSalary;
    }

    private void UpdateByPositionAndSalary(Employee employee, double newSalary)
    {
        var salaries = this.byPositionAndSalary[employee.Position];
        salaries[employee.Salary].Remove(employee);
        if (!salaries.ContainsKey(newSalary))
        {
            salaries[newSalary] = new HashSet<Employee>();
        }

        salaries[newSalary].Add(employee);
    }

    private void UpdateBySalary(Employee employee, double newSalary)
    {
        this.bySalary[employee.Salary].Remove(employee);
        if (!this.bySalary.ContainsKey(newSalary))
        {
            this.bySalary[newSalary] = new HashSet<Employee>();
        }
        this.bySalary[newSalary].Add(employee);
    }

    public IEnumerable<Employee> SearchByFirstName(string firstName)
    {
        if (!this.byFirstName.ContainsKey(firstName))
        {
            return Enumerable.Empty<Employee>();
        }

        return this.byFirstName[firstName];
    }

    public IEnumerable<Employee> SearchByNameAndPosition(string firstName, string lastName, Position position)
    {
        var fullName = firstName + lastName;
        if (!this.byPositionAndName.ContainsKey(position) || !this.byPositionAndName[position].ContainsKey(fullName))
        {
            return Enumerable.Empty<Employee>();
        }

        return this.byPositionAndName[position][fullName];
    }

    public IEnumerable<Employee> SearchByPosition(IEnumerable<Position> positions)
    {
        var linkedList = new LinkedList<Employee>();
        foreach (var position in positions)
        {
            if (this.byPosition.ContainsKey(position))
            {
                foreach (var employee in this.byPosition[position])
                {
                    linkedList.AddLast(employee);
                }
            }
        }

        return linkedList;
    }

    public IEnumerable<Employee> SearchBySalary(double minSalary, double maxSalary)
    {
        var range = this.bySalary.Range(minSalary, true, maxSalary, true);

        if (range.Count == 0)
        {
            return Enumerable.Empty<Employee>();
        }

        var linkedList = new LinkedList<Employee>();
        foreach (var kvp in range)
        {
            foreach (var employee in kvp.Value)
            {
                linkedList.AddLast(employee);
            }
        }

        return linkedList;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private void AddByDate(Employee employee)
    {
        if (!this.byDate.ContainsKey(employee.HireDate))
        {
            this.byDate[employee.HireDate] = new HashSet<Employee>();
        }
        this.byDate[employee.HireDate].Add(employee);
    }

    private void AddByPosition(Employee employee)
    {
        if (!this.byPosition.ContainsKey(employee.Position))
        {
            this.byPosition[employee.Position] = new HashSet<Employee>();
        }
        this.byPosition[employee.Position].Add(employee);
    }

    private void AddBySalary(Employee employee)
    {
        if (!this.bySalary.ContainsKey(employee.Salary))
        {
            this.bySalary[employee.Salary] = new HashSet<Employee>();
        }
        this.bySalary[employee.Salary].Add(employee);
    }

    private void AddByPositionAndSalary(Employee employee)
    {
        if (!this.byPositionAndSalary.ContainsKey(employee.Position))
        {
            this.byPositionAndSalary[employee.Position] = new OrderedDictionary<double, HashSet<Employee>>();
        }

        if (!this.byPositionAndSalary[employee.Position].ContainsKey(employee.Salary))
        {
            this.byPositionAndSalary[employee.Position][employee.Salary] = new HashSet<Employee>();
        }

        this.byPositionAndSalary[employee.Position][employee.Salary].Add(employee);
    }

    private void AddByFirstName(Employee employee)
    {
        if (!this.byFirstName.ContainsKey(employee.FirstName))
        {
            this.byFirstName[employee.FirstName] = new HashSet<Employee>();
        }
        this.byFirstName[employee.FirstName].Add(employee);
    }

    private void AddByPositionAndName(Employee employee)
    {
        var fullName = this.GetFullName(employee);

        if (!this.byPositionAndName.ContainsKey(employee.Position))
        {
            this.byPositionAndName[employee.Position] = new Dictionary<string, HashSet<Employee>>();
        }

        if (!this.byPositionAndName[employee.Position].ContainsKey(fullName))
        {
            this.byPositionAndName[employee.Position][fullName] = new HashSet<Employee>();
        }

        this.byPositionAndName[employee.Position][fullName].Add(employee);
    }

    private string GetFullName(Employee employee)
    {
        return employee.FirstName + employee.LastName;
    }
}