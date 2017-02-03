using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Classes.Models.BaseModel;

namespace CustomClasses.Models
{
    [Table("addresses")]
    public class Address : Base
    {
        #region Attributes

        private string street;
        private string zipCode;
        private string city;
        private string country;

        #endregion

        #region Properties

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("street")]
        public string Street
        {
            get { return street; }
            set
            {
                street = value;
                OnPropertyChanged();
            }
        }

        [Column("zip_code")]
        public string ZipCode
        {
            get { return zipCode; }
            set
            {
                zipCode = value;
                OnPropertyChanged();
            }
        }

        [Column("city")]
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged();
            }
        }

        [Column("country")]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public Address()
        {
        }

        public Address(string street, string zipCode, string city, string country)
        {
            Street = street;
            ZipCode = zipCode;
            City = city;
            Country = country;
        }

        #endregion

        #region Methods

        

        #endregion
    }
}
