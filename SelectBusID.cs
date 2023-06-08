using MessagePack;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FirstAttempt.Models
{
    public class SelectBusID
    {
        [System.ComponentModel.DataAnnotations.Key] public int BusID { get; set; }
    }
}
