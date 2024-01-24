 

namespace Domain.Entities
{
    public class UserRefreshToken
	{
		public long Id { get; set; } 
		public string UserName { get; set; }	 
		public string RefreshToken { get; set; }
		public bool IsActive { get; set; } = true;
	}
}
