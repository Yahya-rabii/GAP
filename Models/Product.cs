using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public float Unitprice { get; set; }
        public float Totalprice { get; set; }

        public string? Name { get; set; }
        public int? ItemsNumber { get; set; }
        public string? Desc { get; set; }
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; } // Foreign key property
        public byte[]? ProductPicture { get; set; }

        public Product() {
        
            Unitprice = 0;
            ItemsNumber = 0;
            Name = string.Empty;
            Desc = string.Empty;
            SupplierId = 0;
            Totalprice= 0;

            ProductPicture = Array.Empty<byte>();
        }
        public Product(int ID , float unitprice, string name, int itemsnum, string desc, int supplierId, float totalprice, byte[] productPicture)
        {
            ProductID = ID;
            Unitprice = unitprice;
            Name = name;
            Desc = desc;
            ItemsNumber = itemsnum;
            SupplierId = supplierId;
            Totalprice = totalprice;
            ProductPicture =productPicture;
        }



    }

}
