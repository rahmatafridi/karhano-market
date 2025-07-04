using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enities
{
    public  class Store : CommonFields
    {
        public string Name { get; set; }    
        public string Description { get; set; } 
        public string Address { get; set; } 
        public Guid TypeId { get; set; }    
        public string Email {  get; set; }  
        public string ContactNo { get; set; }   
        public string ContactPerson { get; set; }   
        public string OwnerName {  get; set; }  
    }
}
