using CsvHelper;
using CsvHelper.Configuration;
using AlleyCatBarbers.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AlleyCatBarbers.Services
{
    public class SpreadsheetReader
    {

        public static List<UserRecord> ReadUserRecords(string filePath)
        {
            var userRecords = new List<UserRecord>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                userRecords = csv.GetRecords<UserRecord>().ToList();
            }

            return userRecords;
        }

    }
}
