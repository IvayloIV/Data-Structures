using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Judge : IJudge
{
    private OrderedSet<int> usersId;
    private OrderedSet<int> contestId;
    private Dictionary<int, Submission> submissions;
    private Dictionary<SubmissionType, HashSet<int>> bySubmissionType;
    private Dictionary<SubmissionType, OrderedDictionary<int, HashSet<Submission>>> byTypeAndPoints;
    private Dictionary<int, HashSet<Submission>> byUserSubmissions;

    public Judge()
    {
        this.usersId = new OrderedSet<int>((a, b) => a.CompareTo(b));
        this.contestId = new OrderedSet<int>((a, b) => a.CompareTo(b));
        this.submissions = new Dictionary<int, Submission>();
        this.bySubmissionType = new Dictionary<SubmissionType, HashSet<int>>();
        this.byTypeAndPoints = new Dictionary<SubmissionType, OrderedDictionary<int, HashSet<Submission>>>();
        this.byUserSubmissions = new Dictionary<int, HashSet<Submission>>();
    }

    public void AddContest(int contestId)
    {
        this.contestId.Add(contestId);
    }

    public void AddSubmission(Submission submission)
    {
        this.CheckExistData(submission.UserId, submission.ContestId);
        if (!this.submissions.ContainsKey(submission.Id))
        {
            this.submissions[submission.Id] = submission;
            this.AddBySubmission(submission);
            this.AddByTypeAndPoints(submission);
            this.AddByUserSubmissions(submission);
        }
    }

    public void AddUser(int userId)
    {
        this.usersId.Add(userId);
    }

    public void DeleteSubmission(int submissionId)
    {
        if (!this.submissions.ContainsKey(submissionId))
        {
            throw new InvalidOperationException();
        }

        var currentSubmission = this.submissions[submissionId];
        this.submissions.Remove(submissionId);
        this.bySubmissionType[currentSubmission.Type].Remove(currentSubmission.ContestId);
        this.byTypeAndPoints[currentSubmission.Type][currentSubmission.Points].Remove(currentSubmission);
        this.byUserSubmissions[currentSubmission.UserId].Remove(currentSubmission);
    }

    public IEnumerable<Submission> GetSubmissions()
    {
        return this.submissions.Values.OrderBy(a => a.Id).ToList();
    }

    public IEnumerable<int> GetUsers()
    {
        return this.usersId;
    }

    public IEnumerable<int> GetContests()
    {
        return this.contestId;
    }

    public IEnumerable<Submission> SubmissionsWithPointsInRangeBySubmissionType(int minPoints, int maxPoints, SubmissionType submissionType)
    {
        if (!this.byTypeAndPoints.ContainsKey(submissionType))
        {
            throw new InvalidOperationException();
        }

        var range = this.byTypeAndPoints[submissionType].Range(minPoints, true, maxPoints, true);
        foreach (var kvp in range)
        {
            foreach (var element in kvp.Value)
            {
                yield return element;
            }
        }
    }

    public IEnumerable<int> ContestsByUserIdOrderedByPointsDescThenBySubmissionId(int userId)
    {
        if (!this.byUserSubmissions.ContainsKey(userId))
        {
            throw new ArgumentException();
        }

        return this.byUserSubmissions[userId]
            .OrderByDescending(a => a.Points)
            .ThenBy(a => a.Id)
            .Select(a => a.ContestId).Distinct();
    }

    public IEnumerable<Submission> SubmissionsInContestIdByUserIdWithPoints(int points, int contestId, int userId)
    {
        if (!this.byUserSubmissions.ContainsKey(userId))
        {
            throw new InvalidOperationException();
        }
        var result = this.byUserSubmissions[userId]
            .Where(a => a.Points == points && a.ContestId == contestId)
            .ToList();
        if (result.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return result;
    }

    public IEnumerable<int> ContestsBySubmissionType(SubmissionType submissionType)
    {
        if (!this.bySubmissionType.ContainsKey(submissionType))
        {
            return Enumerable.Empty<int>();
        }

        return this.bySubmissionType[submissionType];
    }

    private void CheckExistData(int userId, int contestId)
    {
        if (!this.usersId.Contains(userId) || !this.contestId.Contains(contestId))
        {
            throw new InvalidOperationException();
        }
    }

    private void AddBySubmission(Submission submission)
    {
        if (!this.bySubmissionType.ContainsKey(submission.Type))
        {
            this.bySubmissionType[submission.Type] = new HashSet<int>();
        }

        this.bySubmissionType[submission.Type].Add(submission.ContestId);
    }

    private void AddByUserSubmissions(Submission submission)
    {
        if (!this.byUserSubmissions.ContainsKey(submission.UserId))
        {
            this.byUserSubmissions[submission.UserId] = new HashSet<Submission>();
        }
        this.byUserSubmissions[submission.UserId].Add(submission);
    }

    private void AddByTypeAndPoints(Submission submission)
    {
        if (!this.byTypeAndPoints.ContainsKey(submission.Type))
        {
            this.byTypeAndPoints[submission.Type] = new OrderedDictionary<int, HashSet<Submission>>();
        }

        if (!this.byTypeAndPoints[submission.Type].ContainsKey(submission.Points))
        {
            this.byTypeAndPoints[submission.Type][submission.Points] = new HashSet<Submission>();
        }

        this.byTypeAndPoints[submission.Type][submission.Points].Add(submission);
    }
}