using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Classes.Models.BaseModel;

namespace Classes.Models
{
    [Table("users")]
    public class User : Base
    {
        #region Attributes

        private string firstname;
        private string lastname;
        private string login;
        private string password;

        #endregion

        #region Properties

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("firstname")]
        public string Firstname
        {
            get { return firstname; }
            set
            {
                firstname = value;
                OnPropertyChanged();
            }
        }

        [Column("lastname")]
        public string Lastname
        {
            get { return lastname; }
            set
            {
                lastname = value;
                OnPropertyChanged();
            }
        }

        [Column("login")]
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        [Column("password")]
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Data> Datas { get; set; }

        #endregion

        #region Constructors

        public User()
        {
            Roles = new List<Role>();
            Datas = new List<Data>();
        }

        public User(string firstname, string lastname, string password) : this()
        {
            Firstname = firstname;
            Lastname = lastname;
            Login = string.Format($"{Firstname} {Lastname}");
            Password = password;
        }

        #endregion

        #region Methods

        

        #endregion
    }
}
