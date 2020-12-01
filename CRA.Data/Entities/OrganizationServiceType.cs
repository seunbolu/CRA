using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Entities
{
    public class OrganizationServiceType : EntityBase
    {

        [Key]
        public long OrganizationServiceTypeeId { get; set; }

        [Index("IX_OrganizationService_OrganizationId_ServiceTypeId",Order =1)]
        public long OrganizationId { get; set; }

        [Index("IX_OrganizationService_OrganizationId_ServiceTypeId",Order =2)]
        public long ServiceTypeId { get; set; }


        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        [ForeignKey("ServiceTypeId")]
        public virtual ServiceType ServiceType { get; set; }


    }
}
