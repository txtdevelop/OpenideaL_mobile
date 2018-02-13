using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSY.Innovative.Models
{
    interface ILinkable
    {
        string GetLink(int id, LinkType linkType);
    }

    public enum LinkType
    {
        edit,
        view
    }
}
