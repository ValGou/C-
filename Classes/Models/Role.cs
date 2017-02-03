using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Classes.Models.BaseModel;

namespace Classes.Models
{
    [Table("roles")]
    public class Role : Base
    {
        #region Attributes

        private string name;

        #endregion

        #region Properties

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public ICollection<User> Users { get; set; }

        #endregion

        #region Constructors

        public Role()
        {
            Users = new List<User>();
        }

        public Role(string name) : this()
        {
            Name = name;
        }

        #endregion

        #region Methods

        

        #endregion
    }
}
