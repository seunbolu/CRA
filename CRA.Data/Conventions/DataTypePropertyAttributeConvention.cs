using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Conventions
{
    public class DataTypePropertyAttributeConvention
     : PrimitivePropertyAttributeConfigurationConvention<DataTypeAttribute>
    {
        public override void Apply(ConventionPrimitivePropertyConfiguration configuration,
            DataTypeAttribute attribute)
        {
            if (attribute.DataType == DataType.Date)
            {
                configuration.HasColumnType("Date");
            }
        }
    }
}
