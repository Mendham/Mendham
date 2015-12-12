using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Extensions
{
    public interface IIdentityComponentsDeclaringTypeCache : IEntity
    {
        Type GetIdentityComponentsDeclaringType();
    }
}
