﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace JpPublicHolidays
{
    internal class CsvParser : IDisposable
    {
        private readonly CsvReader _csvReader;

        internal CsvParser(TextReader streamReader)
        {
            _csvReader = new CsvReader(streamReader, new Configuration
            {
                CultureInfo = new CultureInfo("ja-JP")
            });
        }

        internal Holiday[] Parse()
        {
            var list = new List<Holiday>();
            while (_csvReader.Read())
            {
                try
                {
                    string name;
                    DateTime date;
                    if (_csvReader.TryGetField(0, out date) && _csvReader.TryGetField(1, out name))
                    {
                        list.Add(new Holiday
                        {
                            Name = name,
                            Date = date
                        });
                    }
                }
                catch
                {
                    //ignore
                }
            }

            return list.OrderBy(x => x.Date).ToArray();
        }

        public void Dispose()
        {
            _csvReader.Dispose();
        }
    }
}
