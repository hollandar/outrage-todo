using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatekeeper.OpenId;
public class OpenIdOptions
{
    public ICollection<OpenIddictApplicationDescriptor> Clients { get; set; }
    public ICollection<OpenIddictScopeDescriptor> Scopes { get; set; }
}
