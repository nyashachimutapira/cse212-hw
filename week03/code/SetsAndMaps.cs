using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        var wordSet = new HashSet<string>(words);
        var result = new List<string>();

        foreach (var word in words)
        {
            char[] chars = word.ToCharArray();
            Array.Reverse(chars);
            string reversed = new string(chars);

            // Check for symmetric pair and that the letters are different
            if (wordSet.Contains(reversed) && word != reversed)
            {
                result.Add($"{word} & {reversed}");
                wordSet.Remove(word); // Remove to avoid duplicates in result
                wordSet.Remove(reversed); // Remove to avoid duplicates in result
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>Dictionary with degree counts</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();

        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            if (fields.Length > 4)
            {
                string degree = fields[3].Trim();
                if (!degrees.ContainsKey(degree))
                {
                    degrees[degree] = 0;
                }
                degrees[degree]++;
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        var cleanedWord1 = word1.Replace(" ", "").ToLower();
        var cleanedWord2 = word2.Replace(" ", "").ToLower();

        if (cleanedWord1.Length != cleanedWord2.Length)
            return false;

        var charCount = new Dictionary<char, int>();

        foreach (var ch in cleanedWord1)
        {
            if (charCount.ContainsKey(ch))
                charCount[ch]++;
            else
                charCount[ch] = 1;
        }

        foreach (var ch in cleanedWord2)
        {
            if (!charCount.ContainsKey(ch) || charCount[ch] == 0)
                return false;
            charCount[ch]--;
        }

        return true;
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        var json = client.GetStringAsync(uri).Result;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);
        var result = new List<string>();

        foreach (var feature in featureCollection.Features)
        {
            result.Add($"{feature.Properties.Place} - Mag {feature.Properties.Mag}");
        }

        return result.ToArray();
    }

    // Classes for deserializing JSON
    public class FeatureCollection
    {
        public Feature[] Features { get; set; }
    }

    public class Feature
    {
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        public string Place { get; set; }
        public double Mag { get; set; }
    }
}