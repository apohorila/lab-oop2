using System;
using System.Collections.Generic;


namespace Parsers
{
    public class RatingService
    {
        private readonly IParsingStrategy _parsingStrategy;

        public RatingService(IParsingStrategy parsingStrategy)
        {
            _parsingStrategy = parsingStrategy;
        }

        public List<Student> LoadRating(string filePath)
        {
            return _parsingStrategy.Parse(filePath);
        }
    }

}