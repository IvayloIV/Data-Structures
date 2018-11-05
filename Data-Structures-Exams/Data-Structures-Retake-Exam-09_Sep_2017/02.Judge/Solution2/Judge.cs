using System;
using System.Collections.Generic;
using System.Linq;

public class Judge : IJudge
{
    private HashSet<int> usersId;
    private HashSet<int> contestId;
    private Dictionary<int, Submission> submissions;
    private Dictionary<SubmissionType, HashSet<int>> bySubmissionType;

    public Judge()
    {
        this.usersId = new HashSet<int>();
        this.contestId = new HashSet<int>();
        this.submissions = new Dictionary<int, Submission>();
        this.bySubmissionType = new Dictionary<SubmissionType, HashSet<int>>();
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
            if (!this.bySubmissionType.ContainsKey(submission.Type))
            {
                this.bySubmissionType[submission.Type] = new HashSet<int>();
            }

            this.bySubmissionType[submission.Type].Add(submission.ContestId);
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
    }

    public IEnumerable<Submission> GetSubmissions()
    {
        return this.submissions.Values.OrderBy(a => a.Id).ToList();
    }

    public IEnumerable<int> GetUsers()
    {
        return this.usersId.OrderBy(a => a);
    }

    public IEnumerable<int> GetContests()
    {
        return this.contestId.OrderBy(a => a);
    }

    public IEnumerable<Submission> SubmissionsWithPointsInRangeBySubmissionType(int minPoints, int maxPoints, SubmissionType submissionType)
    {
        return this.submissions
            .Values
            .Where(a => a.Points >= minPoints && a.Points <= maxPoints && a.Type == submissionType)
            .ToList();
    }

    public IEnumerable<int> ContestsByUserIdOrderedByPointsDescThenBySubmissionId(int userId)
    {
        return this.submissions.Values
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.Points)
            .ThenBy(a => a.Id)
            .Select(a => a.ContestId)
            .Distinct();
    }

    public IEnumerable<Submission> SubmissionsInContestIdByUserIdWithPoints(int points, int contestId, int userId)
    {
        var result = this.submissions
            .Values
            .Where(a => a.Points == points && a.ContestId == contestId && a.UserId == userId)
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
}