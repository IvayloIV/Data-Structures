using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

/// <summary>
/// The ThreadExecutor is the concrete implementation of the IScheduler.
/// You can send any class to the judge system as long as it implements
/// the IScheduler interface. The Tests do not contain any <e>Reflection</e>!
/// </summary>
public class ThreadExecutor : IScheduler
{
    private Dictionary<int, Task> byId;
    private List<Task> tasks;
    private OrderedDictionary<int, HashSet<Task>> byConsumption;

    public ThreadExecutor()
    {
        this.byId = new Dictionary<int, Task>();
        this.tasks = new List<Task>();
        this.byConsumption = new OrderedDictionary<int, HashSet<Task>>();
    }

    private int lowCycles;
    private int highCycles;
    public int Count => this.byId.Count;

    int IScheduler.Count => this.Count;

    public void ChangePriority(int id, Priority newPriority)
    {
        this.CheckExistId(id);
        this.byId[id].TaskPriority = newPriority;
    }

    public bool Contains(Task task)
    {
        return this.byId.ContainsKey(task.Id);
    }

    public int Cycle(int cycles)
    {
        if (this.byId.Count == 0)
        {
            throw new InvalidOperationException();
        }

        this.highCycles += cycles;
        var range = this.byConsumption.Range(this.lowCycles, true, this.highCycles, true);
        this.lowCycles = this.highCycles + 1;

        return this.RemoveTasks(range);
    }

    public void Execute(Task task)
    {
        if (this.byId.ContainsKey(task.Id))
        {
            throw new ArgumentException();
        }

        this.byId[task.Id] = task;
        this.tasks.Add(task);
        this.AddByConsumtion(task);
    }

    public IEnumerable<Task> GetByConsumptionRange(int lo, int hi, bool inclusive)
    {
        IEnumerable<Task> result = null;

        if (inclusive)
        {
            result = this.byId
                .Values
                .Where(a => this.GetUpdatedConsumtion(a) >= lo && this.GetUpdatedConsumtion(a) <= hi);
        } 
        else
        {
            result = this.byId
                .Values
                .Where(a => this.GetUpdatedConsumtion(a) > lo && this.GetUpdatedConsumtion(a) < hi);
        }

        return result.OrderBy(a => this.GetUpdatedConsumtion(a))
                .ThenByDescending(a => a.TaskPriority);
    }

    public Task GetById(int id)
    {
        this.CheckExistId(id);
        return this.byId[id];
    }

    public Task GetByIndex(int index)
    {
        if (index < 0 || index > this.Count - 1)
        {
            throw new ArgumentOutOfRangeException();
        }

        while (!this.byId.ContainsKey(this.tasks[index].Id))
        {
            index++;
        }

        return this.tasks[index];
    }

    public IEnumerable<Task> GetByPriority(Priority type)
    {
        return this.byId
            .Values 
            .Where(a => a.TaskPriority == type)
            .OrderByDescending(a => a.Id);
    }

    public IEnumerable<Task> GetByPriorityAndMinimumConsumption(Priority priority, int lo)
    {
        return this.byId
            .Values
            .Where(a => a.TaskPriority == priority && a.Consumption >= lo)
            .OrderByDescending(a => a.Id);
    }
    
    public IEnumerator<Task> GetEnumerator()
    {
        foreach (var task in this.tasks)
        {
            if (this.byId.ContainsKey(task.Id))
            {
                yield return task;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private void AddByConsumtion(Task task)
    {
        if (!this.byConsumption.ContainsKey(task.Consumption))
        {
            this.byConsumption[task.Consumption] = new HashSet<Task>();
        }

        this.byConsumption[task.Consumption].Add(task);
    }

    private int GetUpdatedConsumtion(Task task)
    {
        return task.Consumption - this.highCycles;
    }

    private void CheckExistId(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }
    }

    private int RemoveTasks(OrderedDictionary<int, HashSet<Task>>.View range)
    {
        var count = 0;
        foreach (var kvp in range)
        {
            foreach (var task in kvp.Value)
            {
                this.byId.Remove(task.Id);
                count++;
            }
        }

        return count;
    }
}
