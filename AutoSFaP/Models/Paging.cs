namespace AutoSFaP.Models
{
    public class Paging
    {
        public int PageNumber { get; set; }
        public int PageLimit { get; set; }
        public int Offset => (PageNumber - 1) * PageLimit;

        public Paging()
        {
        }

        public Paging(int pageNumber, int pageLimit)
        {
            PageNumber = pageNumber;
            PageLimit = pageLimit;
        }
    }
}
