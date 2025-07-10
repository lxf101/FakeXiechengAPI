using System.Text.RegularExpressions;

namespace FakeXiechengAPI.ResourceParameters
{
    public class TouristRouteResourceParameters
    {
        private int _PageNumber = 1;
        public int PageNumber { 
            get {
                return _PageNumber;
            
            }
            set { 
                if(value >= 1)
                {
                    _PageNumber = value;
                }
            } 
        }
        private int _PageSize = 10;
        const int MaxPageSize = 50;
        public int PageSize { 
            get 
            {
                return _PageSize;
            } 
            set { 
                if(value >= 1)
                {
                    _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
                }
            }
        }
        public string Keyword { get; set; }
        private string _rating;
        public string Rating { 
            get { return _rating; } 
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
                    Match match = regex.Match(value);
                    if (match.Success)
                    {
                        RatingOperator = match.Groups[1].Value;
                        RatingValue = Int32.Parse(match.Groups[2].Value);
                    }
                }
                _rating = value; 
            } 
        }
        public string RatingOperator { get; set; }
        public int? RatingValue { get; set; } 
    }
}
