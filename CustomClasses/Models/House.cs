using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Classes.Models.BaseModel;

namespace CustomClasses.Models
{
    [Table("houses")]
    public class House : Base
    {
        #region Attributes

        private int nbRooms;
        private int surface;
        private Address address;

        #endregion

        #region Properties

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nb_rooms")]
        public int NbRooms
        {
            get { return nbRooms; }
            set
            {
                nbRooms = value;
                OnPropertyChanged();
            }
        }

        [Column("surface")]
        public int Surface
        {
            get { return surface; }
            set
            {
                surface = value;
                OnPropertyChanged();
            }
        }

        public Address Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public House()
        {
        }

        public House(int nbRooms, int surface)
        {
            NbRooms = nbRooms;
            Surface = surface;
        }

        #endregion

        #region Methods

        

        #endregion
    }
}
