using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Classes.Models.BaseModel;


namespace Classes.Models
{
    [Table("data")]
    public class Data : Base
    {
        #region Attributes

        private string dataValue;
        private User user;

        #endregion

        #region Properties

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("data_value")]
        public string DataValue
        {
            get { return dataValue; }
            set
            {
                dataValue = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }

        public User User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public Data()
        {
        }

        public Data(string dataValue)
        {
            DataValue = dataValue;
        }

        #endregion

        #region Methods

        

        #endregion
    }
}
