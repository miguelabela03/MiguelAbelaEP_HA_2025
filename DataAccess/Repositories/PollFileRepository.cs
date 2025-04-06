using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DataAccess.Repositories
{
    public class PollFileRepository : IPollRepository
    {
        private string _filePath;

        public PollFileRepository(IConfiguration configuration)
        {
            // Getting the name of the json file from the appsettings.json
            _filePath = configuration["PollsFileName"] ?? "polls.json";
        }

        // Method to add a poll to the JSON file
        public void CreatePoll(Poll poll)
        {
            var polls = GetPollsFromFile();

            // Incrementing the poll id to always have a unique id
            if (polls.Any())
            {
                poll.PollId = polls.Max(x => x.PollId) + 1;
            }
            else
            {
                poll.PollId = 1;
            }

            polls.Add(poll);
            SavePollsToFile(polls);
        }

        // Method that reads all the polls from the json file
        public List<Poll> GetPolls()
        {
            return GetPollsFromFile();
        }

        // Method to reduce code duplication
        private List<Poll> GetPollsFromFile()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Poll>>(jsonData) ?? new List<Poll>();
            }

            return new List<Poll>();
        }

        // Method to reduce code duplication
        private void SavePollsToFile(List<Poll> polls)
        {
            var jsonData = JsonSerializer.Serialize(polls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }

        // Method implemented due to interface
        public void Vote(int pollId, int voteChosen)
        {
            throw new NotImplementedException("Vote functionality is only available for the database.");
        }
    }
}
