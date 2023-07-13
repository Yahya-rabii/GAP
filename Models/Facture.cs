﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GAP.Models
{
    public class Facture
    {
        public int Id { get; set; }
        public Produit Produit { get; set; }
        public double Prix { get; set; }
        public String? FournisseurEmail { get; set; }
        public bool Validite { get; set; }

        [ForeignKey("RespServiceFinance")]
        public int RespServiceFinanceId { get; set; } // Foreign key property

    }
}