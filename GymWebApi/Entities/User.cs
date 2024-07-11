namespace GymWebApi.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserStatus { get; set; }
        public int RoleID { get; set; }
    }
}
