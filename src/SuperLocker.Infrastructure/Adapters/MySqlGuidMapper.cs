using System;
using System.Data;
using Dapper;

namespace SuperLocker.Infrastructure.Adapters
{
    public class MySqlGuidMapper : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid guid)
        {
            parameter.Value = guid.ToString();
        }

        public override Guid Parse(object value)
        {
            return new Guid((string)value);
        }
    }
}