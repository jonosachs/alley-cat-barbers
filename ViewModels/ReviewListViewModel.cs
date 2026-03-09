namespace AlleyCatBarbers.ViewModels
{
    public class ReviewListViewModel
    {
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
        public double AverageRating { get; set; }
    }
}
