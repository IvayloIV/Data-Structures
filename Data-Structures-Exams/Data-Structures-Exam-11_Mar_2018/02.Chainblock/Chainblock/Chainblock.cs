using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Chainblock : IChainblock
{
    private Dictionary<int, Transaction> byId;
    private Dictionary<TransactionStatus, HashSet<Transaction>> byStatus;
    private HashSet<Transaction> transactions;

    public Chainblock()
    {
        this.byId = new Dictionary<int, Transaction>();
        this.byStatus = new Dictionary<TransactionStatus, HashSet<Transaction>>();
        this.transactions = new HashSet<Transaction>();
    }

    public int Count => this.byId.Count;

    public void Add(Transaction tx)
    {
        this.byId[tx.Id] = tx;
        this.AddByStatus(tx);
        this.transactions.Add(tx);
    }    

    public void ChangeTransactionStatus(int id, TransactionStatus newStatus)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        var transaction = this.byId[id];
        this.byStatus[transaction.Status].Remove(transaction);
        transaction.Status = newStatus;
        this.AddByStatus(transaction);
    }

    public bool Contains(Transaction tx)
    {
        return this.byId.ContainsKey(tx.Id);
    }

    public bool Contains(int id)
    {
        return this.byId.ContainsKey(id);
    }

    public IEnumerable<Transaction> GetAllInAmountRange(double lo, double hi)
    {
        return this.transactions.Where(a => a.Amount >= lo && a.Amount <= hi);
    }

    public IEnumerable<Transaction> GetAllOrderedByAmountDescendingThenById()
    {
        return this.byId.Values.OrderByDescending(a => a.Amount).ThenBy(a => a.Id);
    }

    public IEnumerable<string> GetAllReceiversWithTransactionStatus(TransactionStatus status)
    {
        this.CheckStatusExist(status);
        return this.byStatus[status].OrderByDescending(a => a.Amount).Select(a => a.To);
    }

    public IEnumerable<string> GetAllSendersWithTransactionStatus(TransactionStatus status)
    {
        this.CheckStatusExist(status);
        return this.byStatus[status].OrderByDescending(a => a.Amount).Select(a => a.From);
    }

    public Transaction GetById(int id)
    {
        this.CheckExistId(id);
        return this.byId[id];
    }

    public IEnumerable<Transaction> GetByReceiverAndAmountRange(string receiver, double lo, double hi)
    {
        var result = this.byId
            .Values.Where(a => a.To == receiver && a.Amount >= lo && a.Amount < hi)
            .OrderByDescending(a => a.Amount)
            .ThenBy(a => a.Id);

        this.CheckCountForZero(result);

        return result;
    }

    public IEnumerable<Transaction> GetByReceiverOrderedByAmountThenById(string receiver)
    {
        var result = this.byId
            .Values.Where(a => a.To == receiver)
            .OrderByDescending(a => a.Amount)
            .ThenBy(a => a.Id);

        this.CheckCountForZero(result);

        return result;
    }

    public IEnumerable<Transaction> GetBySenderAndMinimumAmountDescending(string sender, double amount)
    {
        var result = this.byId
            .Values
            .Where(a => a.From == sender && a.Amount > amount)
            .OrderByDescending(a => a.Amount);

        this.CheckCountForZero(result);
        return result;
    }

    public IEnumerable<Transaction> GetBySenderOrderedByAmountDescending(string sender)
    {
        var result = this.byId
            .Values
            .Where(a => a.From == sender)
            .OrderByDescending(a => a.Amount);

        this.CheckCountForZero(result);
        return result;
    }

    public IEnumerable<Transaction> GetByTransactionStatus(TransactionStatus status)
    {
        if (!this.byStatus.ContainsKey(status) || this.byStatus[status].Count == 0) 
        {
            throw new InvalidOperationException();
        }

        return this.byStatus[status].OrderByDescending(a => a.Amount);
    }

    public IEnumerable<Transaction> GetByTransactionStatusAndMaximumAmount(TransactionStatus status, double amount)
    {
        if (!this.byStatus.ContainsKey(status))
        {
            return Enumerable.Empty<Transaction>();
        }

        return this.byStatus[status].Where(a => a.Amount <= amount).OrderByDescending(a => a.Amount);
    }

    public IEnumerator<Transaction> GetEnumerator()
    {
        foreach (var transaction in this.transactions)
        {
            yield return transaction;
        }
    }

    public void RemoveTransactionById(int id)
    {
        this.CheckExistId(id);

        var transaction = this.byId[id];
        this.byId.Remove(id);
        this.byStatus[transaction.Status].Remove(transaction);
        this.transactions.Remove(transaction);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private void AddByStatus(Transaction tx)
    {
        if (!this.byStatus.ContainsKey(tx.Status))
        {
            this.byStatus[tx.Status] = new HashSet<Transaction>();
        }

        this.byStatus[tx.Status].Add(tx);
    }

    private void CheckStatusExist(TransactionStatus status)
    {
        if (!this.byStatus.ContainsKey(status) || this.byStatus[status].Count == 0)
        {
            throw new InvalidOperationException();
        }
    }

    private void CheckCountForZero(IOrderedEnumerable<Transaction> result)
    {
        if (result.Count() == 0)
        {
            throw new InvalidOperationException();
        }
    }

    private void CheckExistId(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }
    }
}